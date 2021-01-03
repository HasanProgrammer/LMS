using System;

namespace Common
{
    public partial class Config /*Password*/
    {
        public class Mail
        {
            public string Server       { get; set; }
            public int Port            { get; set; }
            public string FromAddress  { get; set; }
            public string UsernameMail { get; set; }
            public string PasswordMail { get; set; }
        }
    }
    
    public partial class Config /*Password*/
    {
        public class Password
        {
            public bool RequireDigit     { get; set; }
            public bool RequireLowercase { get; set; }
            public bool RequireUppercase { get; set; }
            public int  RequiredLength   { get; set; }
            public string Regex          { get; set; }
            public string RegexMessage   { get; set; }
        }
    }
    
    public partial class Config /*File*/
    {
        public class File
        {
            public int MaxSizeImage  { get; set; }
            public string UploadPath { get; set; }
        }
    }
    
    public partial class Config /*JWT*/
    {
        public class JWT
        {
            public string Issuer   { get; set; } /*صادر کننده*/
            public string Audience { get; set; } /*مصرف کننده*/
            public string Key      { get; set; } /*کلید*/
            public int Expire      { get; set; } /*مدت زمان اعتبار توکن صادر شده*/
        }
    }
    
    public partial class Config /*API's Messages*/
    {
        public class Messages
        {
            //Global
            public string SuccessFetchData { get; set; }
            public string SuccessCreate    { get; set; }
            public string SuccessEdit      { get; set; }
            public string SuccessDelete    { get; set; }
            public string ErrorCreate      { get; set; }
            public string ErrorEdit        { get; set; }
            public string ErrorDelete      { get; set; }
            public string ModelValidation  { get; set; }
            public string UnAuthorized     { get; set; }
            public string TokenIsNotValid  { get; set; }
            public string NotFound         { get; set; }

            //Specific
            public string AlreadyUsedUsername         { get; set; }
            public string UncorrectUsernameOrPassword { get; set; }
            public string UnVerifyEmailAndPhone       { get; set; }
            public string LockedUser                  { get; set; }
            public string SuccessLogin                { get; set; }
            public string SuccessRegister             { get; set; }
            public string SuccessVerifyAccount        { get; set; }
            public string ErrorVerifyAccount          { get; set; }
            public string SuccessVerifyEmail          { get; set; }
            public string ErrorVerifyEmail            { get; set; }
            public string SuccessResetPassword        { get; set; }
            public string ErrorResetPassword          { get; set; }
            public string IsNotUniqueNameField        { get; set; }
            public string IsNotUniqueSlugField        { get; set; }
            public string IsNotUniqueEmailField       { get; set; }
            public string IsNotUniqueUserNameField    { get; set; }
            public string NotFoundImage               { get; set; }
            public string IsNotCorrectImageType       { get; set; }
            public string MaxSizeImage                { get; set; }
        }
    }
    
    public partial class Config /*API's Status Codes*/
    {
        public class StatusCode
        {
            //Global
            public int SuccessFetchData      { get; set; }
            public int SuccessCreate         { get; set; }
            public int SuccessEdit           { get; set; }
            public int SuccessDelete         { get; set; }
            public int ErrorCreate           { get; set; }
            public int ErrorEdit             { get; set; }
            public int ErrorDelete           { get; set; }
            public int ModelValidation       { get; set; }
            public int UnAuthorized          { get; set; }
            public int TokenIsNotValid       { get; set; }
            public int NotFound              { get; set; }

            //Specific
            public int AlreadyUsedUsername          { get; set; }
            public int UncorrectUsernameOrPassword  { get; set; }
            public int UnVerifyEmailAndPhone        { get; set; }
            public int LockedUser                   { get; set; }
            public int SuccessLogin                 { get; set; }
            public int SuccessRegister              { get; set; }
            public int SuccessVerifyAccount         { get; set; }
            public int ErrorVerifyAccount           { get; set; }
            public int SuccessVerifyEmail           { get; set; }
            public int ErrorVerifyEmail             { get; set; }
            public int SuccessResetPassword         { get; set; }
            public int ErrorResetPassword           { get; set; }
            public int IsNotUniqueNameField         { get; set; }
            public int IsNotUniqueUserNameField     { get; set; }
            public int IsNotUniqueEmailField        { get; set; }
            public int IsNotUniqueSlugField         { get; set; }
            public int NotFoundImage                { get; set; }
            public int IsNotCorrectImageType        { get; set; }
            public int IsNotCorrectPassword         { get; set; }
            public int MaxSizeImage                 { get; set; }
        }
    }

    public partial class Config /*Model Validation*/
    {
        public class Validation
        {
            public const string NumberRegex           = @"[1-9][0-9]*";
            public const string NumericRegex          = @"[0-9]*";
            public const string IdentityPasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{12,}$";
        }
    }
    
    public partial class Config /*Routing*/
    {
        public class Routing
        {
            //Base's Route
            public const string BaseRoute = "api/v{version:apiVersion}/";
            
            //Admin's Panel | Categories
            public const string ShowAllCategoriesAdminPanel = "";
            public const string CreateCategoryAdminPanel    = "";
            public const string EditCategoryAdminPanel      = "";
            public const string DeleteCategoryAdminPanel    = "";
            
            //Admin's Panel | Roles
            public const string ShowAllRolesAdminPanel = "";
            public const string CreateRoleAdminPanel   = "";
            public const string EditRoleAdminPanel     = "";
            public const string DeleteRoleAdminPanel   = "";
            
            //Admin's Panel | Users
            public const string ShowAllUsersAdminPanel    = "";
            public const string CreateUserAdminPanel      = "";
            public const string EditUserAdminPanel        = "";
            public const string DeleteUserAdminPanel      = "";
        }
    }
}