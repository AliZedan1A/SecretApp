using System.ComponentModel;
using System.Security.Cryptography;

namespace EncriptionApp.Service
{
    public class EncriptionService
    {
        public event ProgressChangedEventHandler ProgressChanged;
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;
        public bool iscancel = false;
        public bool IsWorngPassword = false;
        public EncriptionService()
        {
        }
        public void Start(string path, string password, bool isEncFun, bool isFolder)
        {
            BackgroundWorker worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            worker.DoWork += (s, e) =>
            {
                if (!isFolder && System.IO.File.Exists(path))
                {
                    try
                    {
                        if (isEncFun)
                        {
                            EncryptFile(path, path + ".encrypted", password);
                            worker.ReportProgress(100, $"تم تشفير الملف: {System.IO.Path.GetFileName(path)}");
                        }
                        else
                        {
                            string outputFile = path.EndsWith(".encrypted")
                                                ? path.Substring(0, path.Length - ".encrypted".Length)
                                                : path;
                            DecryptFile(path, outputFile, password);
                            worker.ReportProgress(100, $"تم فك تشفير الملف: {System.IO.Path.GetFileName(path)}");
                        }
                    }
                    catch (Exception)
                    {
                        worker.ReportProgress(100, $"تخطي الملف: {System.IO.Path.GetFileName(path)}");
                    }
                }
                else if (isFolder && System.IO.Directory.Exists(path))
                {
                    int totalFiles = GetTotalFiles(path);
                    int processed = 0;
                    if (isEncFun)
                    {
                        EncryptDir(path, password, worker, totalFiles, ref processed);
                    }
                    else
                    {
                        DecryptDir(path, password, worker, totalFiles, ref processed);
                    }
                }
            };

            worker.ProgressChanged += (s, e) =>
            {
                ProgressChanged?.Invoke(s, e);
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                RunWorkerCompleted?.Invoke(s, e);
            };

            worker.RunWorkerAsync();
        }

        private int GetTotalFiles(string path)
        {
            try
            {
                return System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories).Length;

            }catch (Exception) 
            {
                return 0; 
            }
        }

        public void EncryptDir(string sourceDir, string password, BackgroundWorker worker, int totalFiles, ref int processed, string targetDir = null)
        {
            if (targetDir == null)
            {
                targetDir = sourceDir + "_enc";
                try
                {
                    System.IO.Directory.CreateDirectory(targetDir);

                }
                catch
                {
                    worker.ReportProgress(0,"acess");
                }
            }
            foreach (string file in System.IO.Directory.GetFiles(sourceDir))
            {
                if (iscancel) return;
                try
                {
                    string targetFile = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(file)) + ".encrypted";
                    EncryptFile(file, targetFile, password);
                    processed++;
                    worker.ReportProgress((int)((processed / (double)totalFiles) * 100), $"done encrypt {System.IO.Path.GetFileName(file)}");
                }
                catch (Exception)
                {
                    processed++; 
                    worker.ReportProgress((int)((processed / (double)totalFiles) * 100), $"FileSkiped: {System.IO.Path.GetFileName(file)}");
                }
            }
            foreach (string directory in System.IO.Directory.GetDirectories(sourceDir))
            {
                if (iscancel) return;
                string subTargetDir = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(directory));
                System.IO.Directory.CreateDirectory(subTargetDir);
                EncryptDir(directory, password, worker, totalFiles, ref processed, subTargetDir);
            }
        }

        public void DecryptDir(string sourceDir, string password, BackgroundWorker worker, int totalFiles, ref int processed, string targetDir = null)
        {
            if (iscancel) return;

            if (targetDir == null)
            {
                if (sourceDir.EndsWith("_enc"))
                    targetDir = sourceDir.Substring(0, sourceDir.Length - "_enc".Length);
                else
                    targetDir = sourceDir + "_dec";
                try
                {
                    System.IO.Directory.CreateDirectory(targetDir);

                }
                catch
                {
                    worker.ReportProgress(0, "acess");

                }
            }
            foreach (string file in System.IO.Directory.GetFiles(sourceDir, "*.encrypted"))
            {
                if (iscancel) return;

                try
                {
                    string targetFile = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileNameWithoutExtension(file));
                    DecryptFile(file, targetFile, password);
                    processed++;
                    worker.ReportProgress((int)((processed / (double)totalFiles) * 100), $"dycript file :  {System.IO.Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    if(ex.Message == "Padding is invalid and cannot be removed.")
                    {
                        iscancel = true;
                        IsWorngPassword = true;
                        return;
                    }
                    processed++;
                    worker.ReportProgress((int)((processed / (double)totalFiles) * 100), $"file skiped : {System.IO.Path.GetFileName(file)}");
                }
            }
            foreach (string directory in System.IO.Directory.GetDirectories(sourceDir))
            {
                if (iscancel) return;

                string subTargetDir = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(directory));
                System.IO.Directory.CreateDirectory(subTargetDir);
                DecryptDir(directory, password, worker, totalFiles, ref processed, subTargetDir);
            }
        }

        private void EncryptFile(string inputFile, string outputFile, string password)
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
               
                    using (var keyGenerator = new Rfc2898DeriveBytes(password, aes.IV, 10000))
                    {
                        aes.Key = keyGenerator.GetBytes(aes.KeySize / 8);
                    }
             
               
                using (System.IO.FileStream outputStream = new System.IO.FileStream(outputFile, System.IO.FileMode.Create))
                {
                    outputStream.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (System.IO.FileStream inputStream = new System.IO.FileStream(inputFile, System.IO.FileMode.Open))
                    {
                        byte[] buffer = new byte[1024 * 1024];
                        int bytesRead;
                        while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cryptoStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

        private void DecryptFile(string inputFile, string outputFile, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[aes.IV.Length];
                using (System.IO.FileStream inputStream = new System.IO.FileStream(inputFile, System.IO.FileMode.Open))
                {
                    inputStream.Read(iv, 0, iv.Length);
                    //try
                    //{
                        using (var keyGenerator = new Rfc2898DeriveBytes(password, iv, 10000))
                        {
                            aes.Key = keyGenerator.GetBytes(aes.KeySize / 8);
                        }
                    //}
                    //catch (Exception ex)
                    //{
                    //    iscancel = true;
                    //    IsWorngPassword = true;
                    //    return;
                    //}

                    aes.IV = iv;
                    using (CryptoStream cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (System.IO.FileStream outputStream = new System.IO.FileStream(outputFile, System.IO.FileMode.Create))
                    {
                        byte[] buffer = new byte[1024 * 1024];
                        int bytesRead;
                        while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outputStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }
    }
}
