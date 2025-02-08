public class LocalizationService
{ 
    public event Action OnLanguageChanged;

    public string CurrentLanguage { get; private set; } = "ar";

    private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
    {
        {
            "ar", new Dictionary<string, string>
            {
                {"Title", "🔒 تشفير وفك تشفير الملفات والمجلدات"},
                {"SelectOperation", "اختر العملية"},
                {"Encrypt", "تشفير"},
                {"Decrypt", "فك التشفير"},
                {"SelectFileOrFolder", "اختر الملف أو المجلد:"},
                {"File", "📄 ملف"},
                {"Folder", "📁 مجلد"},
                {"Password", "كلمة المرور:"},
                {"ConfirmPassword", "تأكيد كلمة المرور:"},
                {"RandomPassword", "كلمة مرور عشوائية"},
                {"StartProcess", "بدء العملية"},
                {"Processing", "جاري المعالجة..."},
                {"SuccessEncrypt", "تم التشفير بنجاح!"},
                {"SuccessDecrypt", "تم فك التشفير بنجاح!"},
                {"NewPath", "📁 المسار الجديد:"},
                {"Error_PasswordMismatch", "كلمة المرور غير متطابقة!"},
                {"Error_InvalidPath", "مسار غير صحيح!"},
                {"Error_NoPathandPassword", "الرجاء تحديد المسار وإدخال كلمة المرور"},
                {"Copied", "تم النسخ!"},
                {"EncryptionPros", "جار التشفير" },
                {"DecryptionPros", "جار فك التشفير" },
                {"PlaceHolderPath", "مسار الملف/المجلد" },
                {"SkipedFiles", "الملفات التي لم يتم تشفيرها لعدم وجود صلاحية " },
                {"noacess","لا يوجد صلاحية الوصول للمجلد" }


            }
        },
        {
            "en", new Dictionary<string, string>
            {
                {"Title", "🔒 Encrypt/Decrypt Files & Folders"},
                {"SelectOperation", "Select Operation"},
                {"Encrypt", "Encrypt"},
                {"Decrypt", "Decrypt"},
                {"SelectFileOrFolder", "Select File/Folder:"},
                {"File", "📄 File"},
                {"Folder", "📁 Folder"},
                {"Password", "Password:"},
                {"ConfirmPassword", "Confirm Password:"},
                {"RandomPassword", "Random Password"},
                {"StartProcess", "Start Process"},
                {"Processing", "Processing..."},
                {"SuccessEncrypt", "Encryption Successful!"},
                {"SuccessDecrypt", "Decryption Successful!"},
                {"NewPath", "📁 New Path:"},
                {"Error_PasswordMismatch", "Passwords don't match!"},
                {"Error_InvalidPath", "Invalid path!"},
                {"Error_NoPathandPassword", "Please specify the path and enter the password\r\n"},
                {"Copied", "Copied!"},
                {"EncryptionPros", "Encryption  in progress" },
                {"DecryptionPros", "Decryption in progress" },
                {"PlaceHolderPath", "Path of the file or folder" },
                {"SkipedFiles", "Files that are not encrypted due to lack of validity " },
                {"noacess","There is no access to the folder" }
            }
        }
    };

    public string this[string key] => _translations[CurrentLanguage].TryGetValue(key, out var value) ? value : key;

    public void SetLanguage(string lang)
    {
        CurrentLanguage = lang;
        OnLanguageChanged?.Invoke();
    }
}