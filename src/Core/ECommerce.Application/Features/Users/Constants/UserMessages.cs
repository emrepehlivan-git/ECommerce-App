namespace ECommerce.Application.Features.Users.Constants;

public static class UserMessages
{
    public const string NotFound = "Kullanıcı bulunamadı.";
    public const string AlreadyExists = "Bu email adresi ile kayıtlı bir kullanıcı zaten mevcut.";
    public const string Created = "Kullanıcı başarıyla oluşturuldu.";
    public const string Updated = "Kullanıcı bilgileri başarıyla güncellendi.";
    public const string Deleted = "Kullanıcı başarıyla silindi.";
    public const string Activated = "Kullanıcı hesabı aktifleştirildi.";
    public const string Deactivated = "Kullanıcı hesabı deaktif edildi.";
    public const string InvalidCredentials = "Geçersiz email veya şifre.";
    public const string AccountLocked = "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.";
    public const string EmailNotConfirmed = "Email adresiniz henüz doğrulanmamış.";
}