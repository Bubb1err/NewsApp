namespace Chatty.Shared.Constants
{
    public static class StringConstants
    {
        public const string DeleteRagAction = "Delete";
        public const string DeleteFileFromIndexAction = "DeleteFile";
        public const int ChatRagMessagesDefaultAmount = 20;
        public const string ConfigurationName = "Configuration";
        public const int KernelRequestRetryAmount = 1;
        public const int KernelRequestTimeoutInMinutes = 1;
        public const int MaxLoadingFileSummaryDays = 1;

        public const string DefaultRunName = "Run #{0}";

        public const string DefaultPromptVendor = "Azure OpenAI";
        public const string DefaultPromptModel = "gpt-3.5-turbo";

        public static class NotificationConstants
        {
            public const string ApiResponseErrorMessage = "Some error occured. Try again later";
            public const int ApiResponseErrorToastDurationInSeconds = 5;
        }

        public static class OCR
        {
            public const string Model = "prebuilt-layout";
            public const string Format = "markdown";
        }

        public static class CollectionModes
        {
            public const string Public = nameof(Public);
            public const string Shared = nameof(Shared);
            public const string Private = nameof(Private);
            public const string PrivateShared = nameof(PrivateShared);
        }

        public const string EmailClaims = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public const string UserIdClaims = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static class Seeder
        {
            public static class ChattyVanilla
            {
                public static class DataSourceTypes
                {
                    public const string FilesFromAzureBlobStorageType = "FilesFromAzureBlobStorage";
                    public const string FilesFromAzureBlobStorageTitle = "Folder";
                }

                public static class ApiClientIds
                {
                    public const string ClientId1 = "a5cacf75-70bb-42bb-8978-e65981f97ba9";
                    public const string ClientId2 = "71a69e91-d9b5-4393-855d-fe6888ea00ab";
                    public const string ClientId3 = "112fd91c-744c-445f-92c4-9b72058d49b0";
                    public const string ClientId4 = "bf592bb5-a568-47ee-9a14-1aee484bc23c";
                }
            }

            public static class Cyprus
            {
                public const string DataSourceTypeId = "8623dbd6-02f5-43eb-891c-f118bb4947b4";
                public const string DataSourceId = "04159993-cb38-4b3e-bfc5-d33b35448282";
                public const string RagId = "2ad94f5d-2575-431a-b78b-69611a35a076";
            }

            public static class Qualisuredx
            {
                public const string RagId = "fc2ce984-fb0b-413f-9bdd-1ade57169021";
            }

            public static class User
            {
                public const string Id = "a50a82c2-0ab6-4bda-9546-a6bcba2b9623";
                public const string Email = "seed@mail.com";
                public const string NormalizedEmail = "SEED@MAIL.COM";
                public const string Password = "Password123!";
            }
        }
        public static class Email
        {
            public const string EmailCodeTemplate = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            public const string ConfirmEmailTemplateConfig = "Confirm Email Template";
            public const string SendChatToEmailTitleConfig = "Send Chat To Email Title";
            public const int CodeExpirationMinutes = 5;
        }
        public static class PathConstants
        {
            public const string MessageAnalysisPath = "message";
            public const string ConversationAnalysisPath = "conversation";
            public const string FileAnalysisPath = "file";
            public const string CVAnalysisPath = "cv";
            public const string InvoiceAnalysisPath = "invoice";
        }

        public class ExcelConstants
        {
            public const string ChatId = "Chat ID";
            public const string ChatTitle = "Chat Title";
            public const string NewChat = "New chat";
            public const string User = "User";
            public const string Bot = "Bot";
            public const string NewSheet = "New Sheet";
        }

        public class NewAccountSetup
        {
            public const string CollectionName = "Examples";
            public static Dictionary<string, string> Prompts = new Dictionary<string, string>
            {
                {
                    "Simple generative; marketing/business", "Create a catchy slogan and a brief marketing campaign for a new eco-friendly shoe company aimed at Gen Z consumers."
                },
                {
                    "Comparision: Zoom vs. Microsoft Teams", "Compare the features, pricing, and customer reviews of Zoom and Microsoft Teams for small businesses. Provide pros and cons for each product and recommend which would be better for a startup with a limited budget"
                },
                {
                    "Comparision: Cloud vendors", "Compare AWS and Microsoft Azure for cloud infrastructure services. Evaluate them based on cost, service reliability, support, and scalability. Recommend the best option for a growing mid-sized company."
                },
                {
                    "Generative: Azure Resource Names", "I need to create an Azure resource group, Azure App Service for the chatbot endpoint, Azure App Service for the configuration panel, Azure AI Service for Language, and Azure Blob storage, Azure FrontDoor.\r\n\r\nI want to use “MMLF” as a project name. If we have multiple services of the same type, please add a purpose to the end of the name. Use lower case for names.\r\n\r\nExample of names:\r\n- Azure Blob Storage: st-mmlf\r\n- Azure Front Door: afd-mmlf\r\n\r\nGenerate a list of Azure resource names using the best practices from Microsoft and format is as a table (Azure service name, Resource name).\r\n\r\nTake abbreviations from the following text:\r\n\r\nAI + machine learning\r\nResource\tResource provider namespace\tAbbreviation\r\nAI Search\tMicrosoft.Search/searchServices\tsrch\r\nAzure AI services\tMicrosoft.CognitiveServices/accounts (kind: AIServices)\tais\r\nAzure AI Studio hub\tMicrosoft.MachineLearningServices/workspaces (kind: Hub)\thub\r\nAzure AI Studio project\tMicrosoft.MachineLearningServices/workspaces (kind: Project)\tproj\r\nAzure AI Video Indexer\tMicrosoft.VideoIndexer/accounts\tavi\r\nAzure Machine Learning workspace\tMicrosoft.MachineLearningServices/workspaces\tmlw\r\nAzure OpenAI Service\tMicrosoft.CognitiveServices/accounts (kind: OpenAI)\toai\r\nBot service\tMicrosoft.BotService/botServices (kind: azurebot)\tbot\r\nComputer vision\tMicrosoft.CognitiveServices/accounts (kind: ComputerVision)\tcv\r\nContent moderator\tMicrosoft.CognitiveServices/accounts (kind: ContentModerator)\tcm\r\nContent safety\tMicrosoft.CognitiveServices/accounts (kind: ContentSafety)\tcs\r\nCustom vision (prediction)\tMicrosoft.CognitiveServices/accounts (kind: CustomVision.Prediction)\tcstv\r\nCustom vision (training)\tMicrosoft.CognitiveServices/accounts (kind: CustomVision.Training)\tcstvt\r\nDocument intelligence\tMicrosoft.CognitiveServices/accounts (kind: FormRecognizer)\tdi\r\nFace API\tMicrosoft.CognitiveServices/accounts (kind: Face)\tface\r\nHealth Insights\tMicrosoft.CognitiveServices/accounts (kind: HealthInsights)\thi\r\nImmersive reader\tMicrosoft.CognitiveServices/accounts (kind: ImmersiveReader)\tir\r\nLanguage service\tMicrosoft.CognitiveServices/accounts (kind: TextAnalytics)\tlang\r\nSpeech service\tMicrosoft.CognitiveServices/accounts (kind: SpeechServices)\tspch\r\nTranslator\tMicrosoft.CognitiveServices/accounts (kind: TextTranslation)\ttrsl\r\nAnalytics and IoT\r\nResource\tResource provider namespace\tAbbreviation\r\nAzure Analysis Services server\tMicrosoft.AnalysisServices/servers\tas\r\nAzure Databricks workspace\tMicrosoft.Databricks/workspaces\tdbw\r\nAzure Data Explorer cluster\tMicrosoft.Kusto/clusters\tdec\r\nAzure Data Explorer cluster database\tMicrosoft.Kusto/clusters/databases\tdedb\r\nAzure Data Factory\tMicrosoft.DataFactory/factories\tadf\r\nAzure Digital Twin instance\tMicrosoft.DigitalTwins/digitalTwinsInstances\tdt\r\nAzure Stream Analytics\tMicrosoft.StreamAnalytics/cluster\tasa\r\nAzure Synapse Analytics private link hub\tMicrosoft.Synapse/privateLinkHubs\tsynplh\r\nAzure Synapse Analytics SQL Dedicated Pool\tMicrosoft.Synapse/workspaces/sqlPools\tsyndp\r\nAzure Synapse Analytics Spark Pool\tMicrosoft.Synapse/workspaces/bigDataPools\tsynsp\r\nAzure Synapse Analytics workspaces\tMicrosoft.Synapse/workspaces\tsynw\r\nData Lake Store account\tMicrosoft.DataLakeStore/accounts\tdls\r\nData Lake Analytics account\tMicrosoft.DataLakeAnalytics/accounts\tdla\r\nEvent Hubs namespace\tMicrosoft.EventHub/namespaces\tevhns\r\nEvent hub\tMicrosoft.EventHub/namespaces/eventHubs\tevh\r\nEvent Grid domain\tMicrosoft.EventGrid/domains\tevgd\r\nEvent Grid subscriptions\tMicrosoft.EventGrid/eventSubscriptions\tevgs\r\nEvent Grid topic\tMicrosoft.EventGrid/domains/topics\tevgt\r\nEvent Grid system topic\tMicrosoft.EventGrid/systemTopics\tegst\r\nHDInsight - Hadoop cluster\tMicrosoft.HDInsight/clusters\thadoop\r\nHDInsight - HBase cluster\tMicrosoft.HDInsight/clusters\thbase\r\nHDInsight - Kafka cluster\tMicrosoft.HDInsight/clusters\tkafka\r\nHDInsight - Spark cluster\tMicrosoft.HDInsight/clusters\tspark\r\nHDInsight - Storm cluster\tMicrosoft.HDInsight/clusters\tstorm\r\nHDInsight - ML Services cluster\tMicrosoft.HDInsight/clusters\tmls\r\nIoT hub\tMicrosoft.Devices/IotHubs\tiot\r\nProvisioning services\tMicrosoft.Devices/provisioningServices\tprovs\r\nProvisioning services certificate\tMicrosoft.Devices/provisioningServices/certificates\tpcert\r\nPower BI Embedded\tMicrosoft.PowerBIDedicated/capacities\tpbi\r\nTime Series Insights environment\tMicrosoft.TimeSeriesInsights/environments\ttsi\r\nCompute and web\r\nResource\tResource provider namespace\tAbbreviation\r\nApp Service environment\tMicrosoft.Web/hostingEnvironments\tase\r\nApp Service plan\tMicrosoft.Web/serverFarms\tasp\r\nAzure Load Testing instance\tMicrosoft.LoadTestService/loadTests\tlt\r\nAvailability set\tMicrosoft.Compute/availabilitySets\tavail\r\nAzure Arc enabled server\tMicrosoft.HybridCompute/machines\tarcs\r\nAzure Arc enabled Kubernetes cluster\tMicrosoft.Kubernetes/connectedClusters\tarck\r\nBatch accounts\tMicrosoft.Batch/batchAccounts\tba\r\nCloud service\tMicrosoft.Compute/cloudServices\tcld\r\nCommunication Services\tMicrosoft.Communication/communicationServices\tacs\r\nDisk encryption set\tMicrosoft.Compute/diskEncryptionSets\tdes\r\nFunction app\tMicrosoft.Web/sites\tfunc\r\nGallery\tMicrosoft.Compute/galleries\tgal\r\nHosting environment\tMicrosoft.Web/hostingEnvironments\thost\r\nImage template\tMicrosoft.VirtualMachineImages/imageTemplates\tit\r\nManaged disk (OS)\tMicrosoft.Compute/disks\tosdisk\r\nManaged disk (data)\tMicrosoft.Compute/disks\tdisk\r\nNotification Hubs\tMicrosoft.NotificationHubs/namespaces/notificationHubs\tntf\r\nNotification Hubs namespace\tMicrosoft.NotificationHubs/namespaces\tntfns\r\nProximity placement group\tMicrosoft.Compute/proximityPlacementGroups\tppg\r\nRestore point collection\tMicrosoft.Compute/restorePointCollections\trpc\r\nSnapshot\tMicrosoft.Compute/snapshots\tsnap\r\nStatic web app\tMicrosoft.Web/staticSites\tstapp\r\nVirtual machine\tMicrosoft.Compute/virtualMachines\tvm\r\nVirtual machine scale set\tMicrosoft.Compute/virtualMachineScaleSets\tvmss\r\nVirtual machine maintenance configuration\tMicrosoft.Maintenance/maintenanceConfigurations\tmc\r\nVM storage account\tMicrosoft.Storage/storageAccounts\tstvm\r\nWeb app\tMicrosoft.Web/sites\tapp\r\nContainers\r\nResource\tResource provider namespace\tAbbreviation\r\nAKS cluster\tMicrosoft.ContainerService/managedClusters\taks\r\nAKS system node pool\tMicrosoft.ContainerService/managedClusters/agentPools (mode: System)\tnpsystem\r\nAKS user node pool\tMicrosoft.ContainerService/managedClusters/agentPools (mode: User)\tnp\r\nContainer apps\tMicrosoft.App/containerApps\tca\r\nContainer apps environment\tMicrosoft.App/managedEnvironments\tcae\r\nContainer registry\tMicrosoft.ContainerRegistry/registries\tcr\r\nContainer instance\tMicrosoft.ContainerInstance/containerGroups\tci\r\nService Fabric cluster\tMicrosoft.ServiceFabric/clusters\tsf\r\nService Fabric managed cluster\tMicrosoft.ServiceFabric/managedClusters\tsfmc\r\nDatabases\r\nResource\tResource provider namespace\tAbbreviation\r\nAzure Cosmos DB database\tMicrosoft.DocumentDB/databaseAccounts/sqlDatabases\tcosmos\r\nAzure Cosmos DB for Apache Cassandra account\tMicrosoft.DocumentDB/databaseAccounts\tcoscas\r\nAzure Cosmos DB for MongoDB account\tMicrosoft.DocumentDB/databaseAccounts\tcosmon\r\nAzure Cosmos DB for NoSQL account\tMicrosoft.DocumentDb/databaseAccounts\tcosno\r\nAzure Cosmos DB for Table account\tMicrosoft.DocumentDb/databaseAccounts\tcostab\r\nAzure Cosmos DB for Apache Gremlin account\tMicrosoft.DocumentDb/databaseAccounts\tcosgrm\r\nAzure Cosmos DB PostgreSQL cluster\tMicrosoft.DBforPostgreSQL/serverGroupsv2\tcospos\r\nAzure Cache for Redis instance\tMicrosoft.Cache/Redis\tredis\r\nAzure SQL Database server\tMicrosoft.Sql/servers\tsql\r\nAzure SQL database\tMicrosoft.Sql/servers/databases\tsqldb\r\nAzure SQL Elastic Job agent\tMicrosoft.Sql/servers/jobAgents\tsqlja\r\nAzure SQL Elastic Pool\tMicrosoft.Sql/servers/elasticpool\tsqlep\r\nMariaDB server\tMicrosoft.DBforMariaDB/servers\tmaria\r\nMariaDB database\tMicrosoft.DBforMariaDB/servers/databases\tmariadb\r\nMySQL database\tMicrosoft.DBforMySQL/servers\tmysql\r\nPostgreSQL database\tMicrosoft.DBforPostgreSQL/servers\tpsql\r\nSQL Server Stretch Database\tMicrosoft.Sql/servers/databases\tsqlstrdb\r\nSQL Managed Instance\tMicrosoft.Sql/managedInstances\tsqlmi\r\nDeveloper tools\r\nResource\tResource provider namespace\tAbbreviation\r\nApp Configuration store\tMicrosoft.AppConfiguration/configurationStores\tappcs\r\nMaps account\tMicrosoft.Maps/accounts\tmap\r\nSignalR\tMicrosoft.SignalRService/SignalR\tsigr\r\nWebPubSub\tMicrosoft.SignalRService/webPubSub\twps\r\nDevOps\r\nResource\tResource provider namespace\tAbbreviation\r\nAzure Managed Grafana\tMicrosoft.Dashboard/grafana\tamg\r\nIntegration\r\nResource\tResource provider namespace\tAbbreviation\r\nAPI management service instance\tMicrosoft.ApiManagement/service\tapim\r\nIntegration account\tMicrosoft.Logic/integrationAccounts\tia\r\nLogic app\tMicrosoft.Logic/workflows\tlogic\r\nService Bus namespace\tMicrosoft.ServiceBus/namespaces\tsbns\r\nService Bus queue\tMicrosoft.ServiceBus/namespaces/queues\tsbq\r\nService Bus topic\tMicrosoft.ServiceBus/namespaces/topics\tsbt\r\nService Bus topic subscription\tMicrosoft.ServiceBus/namespaces/topics/subscriptions\tsbts\r\nManagement and governance\r\nResource\tResource provider namespace\tAbbreviation\r\nAutomation account\tMicrosoft.Automation/automationAccounts\taa\r\nAzure Policy definition\tMicrosoft.Authorization/policyDefinitions\t<descriptive>\r\nApplication Insights\tMicrosoft.Insights/components\tappi\r\nAzure Monitor action group\tMicrosoft.Insights/actionGroups\tag\r\nAzure Monitor data collection rule\tMicrosoft.Insights/dataCollectionRules\tdcr\r\nAzure Monitor alert processing rule\tMicrosoft.AlertsManagement/actionRules\tapr\r\nBlueprint (planned for deprecation)\tMicrosoft.Blueprint/blueprints\tbp\r\nBlueprint assignment (planned for deprecation)\tMicrosoft.Blueprint/blueprints/artifacts\tbpa\r\nData collection endpoint\tMicrosoft.Insights/dataCollectionEndpoints\tdce\r\nLog Analytics workspace\tMicrosoft.OperationalInsights/workspaces\tlog\r\nLog Analytics query packs\tMicrosoft.OperationalInsights/querypacks\tpack\r\nManagement group\tMicrosoft.Management/managementGroups\tmg\r\nMicrosoft Purview instance\tMicrosoft.Purview/accounts\tpview\r\nResource group\tMicrosoft.Resources/resourceGroups\trg\r\nTemplate specs name\tMicrosoft.Resources/templateSpecs\tts\r\nMigration\r\nResource\tResource provider namespace\tAbbreviation\r\nAzure Migrate project\tMicrosoft.Migrate/assessmentProjects\tmigr\r\nDatabase Migration Service instance\tMicrosoft.DataMigration/services\tdms\r\nRecovery Services vault\tMicrosoft.RecoveryServices/vaults\trsv\r\nNetworking\r\nResource\tResource provider namespace\tAbbreviation\r\nApplication gateway\tMicrosoft.Network/applicationGateways\tagw\r\nApplication security group (ASG)\tMicrosoft.Network/applicationSecurityGroups\tasg\r\nCDN profile\tMicrosoft.Cdn/profiles\tcdnp\r\nCDN endpoint\tMicrosoft.Cdn/profiles/endpoints\tcdne\r\nConnections\tMicrosoft.Network/connections\tcon\r\nDNS\tMicrosoft.Network/dnsZones\t<DNS domain name>\r\nDNS forwarding ruleset\tMicrosoft.Network/dnsForwardingRulesets\tdnsfrs\r\nDNS private resolver\tMicrosoft.Network/dnsResolvers\tdnspr\r\nDNS private resolver inbound endpoint\tMicrosoft.Network/dnsResolvers/inboundEndpoints\tin\r\nDNS private resolver outbound endpoint\tMicrosoft.Network/dnsResolvers/outboundEndpoints\tout\r\nDNS zone\tMicrosoft.Network/privateDnsZones\t<DNS domain name>\r\nFirewall\tMicrosoft.Network/azureFirewalls\tafw\r\nFirewall policy\tMicrosoft.Network/firewallPolicies\tafwp\r\nExpressRoute circuit\tMicrosoft.Network/expressRouteCircuits\terc\r\nExpressRoute gateway\tMicrosoft.Network/virtualNetworkGateways\tergw\r\nFront Door (Standard/Premium) profile\tMicrosoft.Cdn/profiles\tafd\r\nFront Door (Standard/Premium) endpoint\tMicrosoft.Cdn/profiles/afdEndpoints\tfde\r\nFront Door firewall policy\tMicrosoft.Network/frontdoorWebApplicationFirewallPolicies\tfdfp\r\nFront Door (classic)\tMicrosoft.Network/frontDoors\tafd\r\nIP group\tMicrosoft.Network/ipGroups\tipg\r\nLoad balancer (internal)\tMicrosoft.Network/loadBalancers\tlbi\r\nLoad balancer (external)\tMicrosoft.Network/loadBalancers\tlbe\r\nLoad balancer rule\tMicrosoft.Network/loadBalancers/inboundNatRules\trule\r\nLocal network gateway\tMicrosoft.Network/localNetworkGateways\tlgw\r\nNAT gateway\tMicrosoft.Network/natGateways\tng\r\nNetwork interface (NIC)\tMicrosoft.Network/networkInterfaces\tnic\r\nNetwork security group (NSG)\tMicrosoft.Network/networkSecurityGroups\tnsg\r\nNetwork security group (NSG) security rules\tMicrosoft.Network/networkSecurityGroups/securityRules\tnsgsr\r\nNetwork Watcher\tMicrosoft.Network/networkWatchers\tnw\r\nPrivate Link\tMicrosoft.Network/privateLinkServices\tpl\r\nPrivate endpoint\tMicrosoft.Network/privateEndpoints\tpep\r\nPublic IP address\tMicrosoft.Network/publicIPAddresses\tpip\r\nPublic IP address prefix\tMicrosoft.Network/publicIPPrefixes\tippre\r\nRoute filter\tMicrosoft.Network/routeFilters\trf\r\nRoute server\tMicrosoft.Network/virtualHubs\trtserv\r\nRoute table\tMicrosoft.Network/routeTables\trt\r\nService endpoint policy\tMicrosoft.serviceEndPointPolicies\tse\r\nTraffic Manager profile\tMicrosoft.Network/trafficManagerProfiles\ttraf\r\nUser defined route (UDR)\tMicrosoft.Network/routeTables/routes\tudr\r\nVirtual network\tMicrosoft.Network/virtualNetworks\tvnet\r\nVirtual network gateway\tMicrosoft.Network/virtualNetworkGateways\tvgw\r\nVirtual network manager\tMicrosoft.Network/networkManagers\tvnm\r\nVirtual network peering\tMicrosoft.Network/virtualNetworks/virtualNetworkPeerings\tpeer\r\nVirtual network subnet\tMicrosoft.Network/virtualNetworks/subnets\tsnet\r\nVirtual WAN\tMicrosoft.Network/virtualWans\tvwan\r\nVirtual WAN Hub\tMicrosoft.Network/virtualHubs\tvhub\r\nSecurity\r\nResource\tResource provider namespace\tAbbreviation\r\nAzure Bastion\tMicrosoft.Network/bastionHosts\tbas\r\nKey vault\tMicrosoft.KeyVault/vaults\tkv\r\nKey Vault Managed HSM\tMicrosoft.KeyVault/managedHSMs\tkvmhsm\r\nManaged identity\tMicrosoft.ManagedIdentity/userAssignedIdentities\tid\r\nSSH key\tMicrosoft.Compute/sshPublicKeys\tsshkey\r\nVPN Gateway\tMicrosoft.Network/vpnGateways\tvpng\r\nVPN connection\tMicrosoft.Network/vpnGateways/vpnConnections\tvcn\r\nVPN site\tMicrosoft.Network/vpnGateways/vpnSites\tvst\r\nWeb Application Firewall (WAF) policy\tMicrosoft.Network/firewallPolicies\twaf\r\nWeb Application Firewall (WAF) policy rule group\tMicrosoft.Network/firewallPolicies/ruleGroups\twafrg\r\nStorage\r\nResource\tResource provider namespace\tAbbreviation\r\nAzure StorSimple\tMicrosoft.StorSimple/managers\tssimp\r\nBackup Vault name\tMicrosoft.DataProtection/backupVaults\tbvault\r\nBackup Vault policy\tMicrosoft.DataProtection/backupVaults/backupPolicies\tbkpol\r\nFile share\tMicrosoft.Storage/storageAccounts/fileServices/shares\tshare\r\nStorage account\tMicrosoft.Storage/storageAccounts\tst\r\nStorage Sync Service name\tMicrosoft.StorageSync/storageSyncServices\tsss\r\nVirtual desktop infrastructure\r\nResource\tResource provider namespace\tAbbreviation\r\nVirtual desktop host pool\tMicrosoft.DesktopVirtualization/hostPools\tvdpool\r\nVirtual desktop application group\tMicrosoft.DesktopVirtualization/applicationGroups\tvdag\r\nVirtual desktop workspace\tMicrosoft.DesktopVirtualization/workspaces\tvdws\r\nVirtual desktop scaling plan\tMicrosoft.DesktopVirtualization/scalingPlans\tvdscaling"
                },
                {
                    "Generative: create contract template", "Create a professional contract template. The contract should include clear and formal language, covering all key aspects of the working relationship between the Contractor and the Client. The contract must include the following sections:\r\n\r\nIntroduction: A brief introduction to the contract, including the names of the parties involved (the 'Contractor' and the 'Client'), the date the contract is entered into, and a statement outlining the general nature of the agreement.\r\n\r\nScope of Work: A section describing in detail the specific services the Contractor will provide, including project deliverables, any agreed-upon milestones, and the timeline for completion.\r\n\r\nCompensation: A clear statement of the Contractor's fees, whether it's based on an hourly rate or a flat fee, and how payments will be structured (e.g., upfront, in installments, upon completion). Include any terms for late payments and possible penalties.\r\n\r\nRevisions and Feedback: Define how many rounds of revisions are included in the initial scope of work and how additional revisions will be handled (e.g., extra fees for revisions beyond a set number).\r\n\r\nIntellectual Property Rights: Specify who owns the intellectual property created under the agreement, whether the Contractor retains rights to the work until full payment is made, and what rights the Client will receive after the project is complete.\r\n\r\nConfidentiality: Include a clause where both parties agree to keep all confidential information shared during the project private, with a clear definition of what constitutes confidential information.\r\n\r\nTermination of Agreement: Outline the conditions under which either party can terminate the contract, such as non-performance or breach of agreement. Specify any notice periods and any compensation owed upon early termination.\r\n\r\nDispute Resolution: Detail the process for handling disputes between the Contractor and the Client, such as mediation or arbitration, and the jurisdiction in which disputes will be handled.\r\n\r\nSignatures: Provide spaces for the Contractor and the Client to sign and date the contract, with language stating that by signing, both parties agree to the terms outlined in the document.\r\n\r\nThe contract should be written in a clear and concise manner, suitable for both parties who may not be legal experts. Include placeholders for the relevant names, dates, and other variable information, and ensure the language is professional, formal, and legally sound."
                },
            };
        }

        public class BlobStorageConstants
        {
            public const string UserProfilePhotosContainerName = "profile-photos";
        }
    }
}
