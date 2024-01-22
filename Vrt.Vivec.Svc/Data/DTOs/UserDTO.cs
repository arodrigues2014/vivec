
public class UserDTO
{
    public int id { get; set; }
    public bool canRequestHolidays { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public object phoneNumber { get; set; }
    public string language { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string avatarUrl { get; set; }
    public string avatarThumbnailSUrl { get; set; }
    public string avatarThumbnailMUrl { get; set; }
    public int state { get; set; }
    public string[] roles { get; set; }
    public Editorpermissions editorPermissions { get; set; }
    public Company company { get; set; }
    public object groupsIds { get; set; }
    public Privateinfo privateInfo { get; set; }
}

public class Editorpermissions
{
    public int availableFeeds { get; set; }
    public bool highlightContentAllowed { get; set; }
    public bool publishContentAllowed { get; set; }
    public int pushOptions { get; set; }
    public bool viewStatisticsAllowed { get; set; }
    public int allowedCategoriesKind { get; set; }
    public object[] allowedCategories { get; set; }
}

public class Company
{
    public int id { get; set; }
    public int color { get; set; }
    public object name { get; set; }
    public string termsOfUse { get; set; }
    public string logoURL { get; set; }
    public string emailLogoURL { get; set; }
    public string brand { get; set; }
    public Configuration configuration { get; set; }
}

public class Configuration
{
    public bool showSocialLinks { get; set; }
    public bool externalLoginEnabled { get; set; }
    public Companyappmodules companyAppModules { get; set; }
    public string ratingKind { get; set; }
    public int requiredNotificationChannel { get; set; }
    public string timeZone { get; set; }
    public string defaultLanguage { get; set; }
    public string[] availableLanguages { get; set; }
    public string appName { get; set; }
    public string appStore { get; set; }
    public string googlePlay { get; set; }
    public string appAndroidVersion { get; set; }
    public string appIOSVersion { get; set; }
    public bool iosRedemptionActive { get; set; }
    public int loginKind { get; set; }
    public int commentsDefault { get; set; }
    public int sharingDefault { get; set; }
    public int ratingDefault { get; set; }
    public Webappconfiguration webAppConfiguration { get; set; }
    public Appconfiguration appConfiguration { get; set; }
    public Timeoffconfiguration timeOffConfiguration { get; set; }
    public Documentsconfiguration documentsConfiguration { get; set; }
    public Loginconfiguration loginConfiguration { get; set; }
    public Customnamings customNamings { get; set; }
    public string groupChatServiceName { get; set; }
    public bool multiCountry { get; set; }
}

public class Companyappmodules
{
    public Knowledgecenter knowledgeCenter { get; set; }
    public Conversations conversations { get; set; }
    public Experiences experiences { get; set; }
    public Webapp webapp { get; set; }
    public Timeoff timeOff { get; set; }
    public Documents documents { get; set; }
    public Employeedirectory employeeDirectory { get; set; }
    public Timetracking timeTracking { get; set; }
}

public class Knowledgecenter
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}

public class Conversations
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}

public class Experiences
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}

public class Webapp
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}

public class Timeoff
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}

public class Documents
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
    public bool productionMode { get; set; }
    public bool backofficeHidden { get; set; }
    public int totalSignatures { get; set; }
    public int availableSignatures { get; set; }
    public object expirationDate { get; set; }
}


public class Employeedirectory
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}

public class Timetracking
{
    public int id { get; set; }
    public int module { get; set; }
    public int state { get; set; }
    public bool enabledByDefault { get; set; }
}


public class Webappconfiguration
{
    public bool enabled { get; set; }
    public string headerLogoUrl { get; set; }
    public string loginLogoUrl { get; set; }
    public int headerBgColor { get; set; }
}

public class Appconfiguration
{
    public bool negative { get; set; }
    public int corporateColor { get; set; }
    public int highlightedColor { get; set; }
    public int corporateGradientEnd { get; set; }
    public int highlightedGradientEnd { get; set; }
    public string logoUrl { get; set; }
}

public class Timeoffconfiguration
{
    public int approvalFlow { get; set; }
    public bool responsibleCanManageAbsences { get; set; }
    public bool allowsHalfDay { get; set; }
}

public class Documentsconfiguration
{
    public int documentOpener { get; set; }
    public bool allowDownloading { get; set; }
}

public class Loginconfiguration
{
    public int id { get; set; }
    public int ctaLayoutKind { get; set; }
    public Dialengaloginheadercontents dialengaLoginHeaderContents { get; set; }
    public Dialengamainctacontents dialengaMainCtaContents { get; set; }
    public Dialengalogintextcontents dialengaLoginTextContents { get; set; }
    public Dialengadefaulttextcontents dialengaDefaultTextContents { get; set; }
    public Ssoconfig[] ssoConfigs { get; set; }
}

public class Dialengaloginheadercontents
{
    public object[] availableLanguages { get; set; }
    public Contents contents { get; set; }
}

public class Contents
{
}

public class Dialengamainctacontents
{
    public object[] availableLanguages { get; set; }
    public Contents1 contents { get; set; }
}

public class Contents1
{
}

public class Dialengalogintextcontents
{
    public object[] availableLanguages { get; set; }
    public Contents2 contents { get; set; }
}

public class Contents2
{
}

public class Dialengadefaulttextcontents
{
    public object[] availableLanguages { get; set; }
    public Contents3 contents { get; set; }
}

public class Contents3
{
}

public class Ssoconfig
{
    public int id { get; set; }
    public string name { get; set; }
    public int order { get; set; }
    public string provider { get; set; }
    public int kind { get; set; }
    public int availabilityKind { get; set; }
    public string[] availableLanguages { get; set; }
    public string loginURL { get; set; }
    public string logoutURL { get; set; }
    public Contents4 contents { get; set; }
    public string clientId { get; set; }
    public object realm { get; set; }
    public string clientSecret { get; set; }
    public string issuer { get; set; }
    public object tenantId { get; set; }
    public string scope { get; set; }
    public string responseType { get; set; }
    public object extraParams { get; set; }
    public bool skipRefreshToken { get; set; }
    public string usernameClaim { get; set; }
    public string redirectUriTokenName { get; set; }
    public object regex { get; set; }
    public object usernameGroup { get; set; }
}

public class Contents4
{
    public Es es { get; set; }
}

public class Es
{
    public string label { get; set; }
}

public class Customnamings
{
    public KnowledgecenterTitle knowledgeCentertitle { get; set; }
}

public class KnowledgecenterTitle
{
    public string en { get; set; }
    public string es { get; set; }
    public string ca_ES { get; set; }
}

public class Privateinfo
{
    public string nickname { get; set; }
    public object birthDate { get; set; }
    public object birthPlace { get; set; }
    public object nationality { get; set; }
    public object maritalStatus { get; set; }
    public object children { get; set; }
    public object socialSecurityNumber { get; set; }
    public object bankAccount { get; set; }
}
