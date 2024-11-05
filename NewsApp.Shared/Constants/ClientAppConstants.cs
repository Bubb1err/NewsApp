// Copyright (c) Microsoft. All rights reserved.

namespace Chatty.Shared.Constants;

public class ClientAppConstants
{
    public static class PromptSettings
    {
        public const double DefaultTemperature = 0.1;

        public static class Deployments
        {
            public const string GPT4o = "gpt-4o";
            public const string GPT4oMini = "gpt-4o-mini";
            public const string GPT35 = "gpt-35-turbo";
        }

        public static class CalculateCost
        {
            public const int DefaultRound = 5;
            public const double GPT4oModelInputToken = 0.0025;
            public const double GPT4oModelCompletionToken = 0.01;

            public const double GPT4oMiniModelInputToken = 0.00015;
            public const double GPT4oModelMiniCompletionToken = 0.0006;

            public const double GPT35ModelInputToken = 0.001;
            public const double GPT35ModelCompletionToken = 0.002;
        }
    }

    public static class Errors
    {
        public const string CitationNotFound = "Citation not found";
    }

    public static class Cyprus
    {
        public static class Indexes
        {
            public const string Cyprus = "cypruslaws";
        }

        public static class Authentication
        {
            public const string ApiKey = "64c5ca85-6d25-4efe-97c0-33d9a4c080e1";
        }

        public const string EnglishLanguage = "English";
        public const string GreekLanguage = "Greek";
    }

    /// <summary>
    /// TODO
    /// </summary>
    public class UI
    {
        public static class Guest
        {
            public static class Chat
            {
                public const string WelcomeBotMessage = "Hello, Welcome to ThyroidGuide.AI. I can answer any of the questions related to the thyroid disease that you are interested in, any new diagnostic tests, treatment guidelines, and current clinical trials. My answers are based on all of the latest published recommendations from experts around the world.";
            }
        }

        public class Qualisure
        {
            public const string QualisureBaseUrl = "https://qualisuredx-ai-poc-api.azurewebsites.net";
            public const string QualisureRagId = "fc2ce984-fb0b-413f-9bdd-1ade57169021";
            public const int TopK = 6;
        }
        public class Chatty
        {
            public const string ChattyBaseUrlLocal = "https://localhost:7173/";
            public const string ChattyChatEndpoint = "api/Rags/chat";
            public const string ChattyTokenRefreshEndpoint = "/api/Authentication/Refresh";
            public const string ChattyAuthEndpoint = "/api/Authentication/login";
            public const string ChattyGetThreadsEndpoint = "/api/Threads/list";
            public const string ChattyThreadsEndpoint = "/api/Threads/";
            public const string ChattyFeedbackEndpoint = "/api/Feedback/";
            public const string ChattyGetBookmarksEndpoint = "/api/Bookmark/list";
            public const string ChattyBookmarksEndpoint = "/api/Bookmark";
            public const string ChattyEmailSendMessagesEndpoint = "/send-messages";
            public const string ChattyDataSourcesEndpoint = "api/datasource";
            public const string ChattyDataSourceTypesEndpoint = "api/datasourcetype";
            public const string ChattyModelsEndpoint = "api/models";
            public const string ChattyPromptsEndpoint = "api/prompts";
            public const string ChattyProjectsEndpoint = "api/projects";
            public const string ChattyCollectionsEndpoint = "api/collections";
            public const string ChattyCollectionsPersonalEndpoint = "api/collections/personal";
            public const string ChattyInvokePromptEndpoint = "api/prompts/invoke-prompt";
            public const string ChattyRunsEndpoint = "api/runs";
            public const string ChattyPromptCommentsEndpoint = "api/promptcomments";
            public const string ChattyRagsEndpoint = "api/rags";
            public const string ChattyRagsChatEndpoint = "api/rags/chat";
            public const string ChattyIndexRagEndpoint = "api/rags/index";
            public const string ChattyUpdateRunApproveEndpoint = "api/runs/UpdateRunApprove";
            public const string ChattyUpdateRunNameEndpoint = "api/runs/UpdateRunName";
            public const string ChattyDeleteRunEndpoint = "api/runs/DeleteRun";
            public const string ChattyDataSourceFilesEndpoint = "api/datasourcefiles";
            public const string ChattyGetPromptsFromCollectionsEndpoint = "api/prompts/list";
            public const string ChattyDeleteFileEndpoint = "api/datasourcefiles/deletefile";
            public const string ChattyCollectionSharesEndpoint = "api/collectionshares";
            public const string ChattyUpdateRagEndpoint = "api/rags/index";
            public const string ChattyAnalysisEndpoint = "api/analysis";
            public const string ChattyMessagesEndpoint = "api/messages";
            public const string GetBlobEndpoint = "api/BlobStorage/get";
            public const string GetBlobOrDefaultEndpoint = "api/BlobStorage/getOrDefault";
            public const string UploadBlobEndpoint = "api/BlobStorage/upload";
            public const string DeleteBlobEndpoint = "api/BlobStorage/delete";
            public const string CreateContainerEndpoint = "api/BlobStorage/createContainer";
            public const string GenerateValidationCodeEndpoint = "api/email/code/confirmation";
            public const string ValidateCodeEndpoint = "api/email/code/validate";
            public const string ConfirmEmailEndpoint = "api/email/code/confirmEmail";
            public const string RegisterTempUser = "api/authentication/registerTempUser";
            public const string ChangeUserEmail = "api/authentication/changeUserEmail";
            public const string ChangeUserPassword = "api/authentication/changeUserPassword";
            public const string ClientConfigurationList = "api/clientconfiguration/list";
            public const string ClientConfiguration = "api/clientconfiguration";
            public const string ExportChatsToExcelEndpoint = "api/rags/export-chats";
            public const string UsersEndpoint = "api/user";
            public const string UpdateUserProfileEndpoint = "api/user/profile";
        }
    }
}
