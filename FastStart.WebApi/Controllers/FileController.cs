using FastStart.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Cryptography;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 文件操作
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "File")]
    public class FileController : ControllerBase
    {
        private readonly HashSet<string> _fileHashes = new HashSet<string>();

        private async Task<string> ComputeFileHashAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var sha256 = SHA256.Create();
            var hash = await sha256.ComputeHashAsync(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("fileUpload")]
        [AllowAnonymous]
        public async Task<ResultModel<bool>> FileUpload(List<IFormFile> files)
        {
            if (files == null || !files.Any())
            {
                return ResultModel<bool>.Fail("The file is empty or does not exist", false);
            }
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "fileuploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in files)
            {
                var fileHash = await ComputeFileHashAsync(file);
                // 检查哈希值是否已存在
                if (_fileHashes.Contains(fileHash))
                {
                    // 如果文件哈希已存在，则跳过上传或返回错误信息
                    return ResultModel<bool>.Fail("检测到重复文件", false);
                }
                // 生成唯一文件名以防止文件名冲突,Path.GetExtension获取指定文件的扩展名
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadPath, fileName);
                try
                {
                    // 验证文件名是否包含无效字符，防止路径遍历
                    if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    {
                        return ResultModel<bool>.Fail("无效的文件名", false);
                    }

                    // 如果需要，这里可以实现文件大小和类型检查

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        // 异步复制文件内容到服务器
                        await file.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    // 记录异常信息
                    Log.Error("File upload exception" + ex);
                    // 根据异常返回有意义的错误信息
                    return ResultModel<bool>.Fail(StatusCodes.Status500InternalServerError, "File upload exception", false);
                }
            }
            return ResultModel<bool>.Success(true);
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("downloadFile/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "fileuploads", fileName);

            if (System.IO.File.Exists(filePath))
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, "application/octet-stream", Path.GetFileName(filePath));
            }
            else
            {
                return NotFound();
            }
        }
    }
}