param appName string
param keyVaultName string
param location string = resourceGroup().location
param sku string = 'Shared'
param skuCode string = 'D1'
param workerSize string = '0'
param workerSizeId string = '0'
param numberOfWorkers string = '1'
param currentStack string = 'dotnetcore'
param applicationInsightsName string = appName


resource app 'Microsoft.Web/sites@2018-02-01' = {
  name: appName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    siteConfig: {
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: reference(app_insights.id, '2015-05-01').InstrumentationKey
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'default'
        }
        {
          name: 'DiagnosticServices_EXTENSION_VERSION'
          value: 'disabled'
        }
        {
          name: 'APPINSIGHTS_PROFILERFEATURE_VERSION'
          value: 'disabled'
        }
        {
          name: 'APPINSIGHTS_SNAPSHOTFEATURE_VERSION'
          value: 'disabled'
        }
        {
          name: 'InstrumentationEngine_EXTENSION_VERSION'
          value: 'disabled'
        }
        {
          name: 'SnapshotDebugger_EXTENSION_VERSION'
          value: 'disabled'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_BaseExtensions'
          value: 'disabled'
        }
      ]
      metadata: [
        {
          name: 'CURRENT_STACK'
          value: currentStack
        }
      ]
    }
    serverFarmId: hostingPlanName.id
    clientAffinityEnabled: true
    httpsOnly: true
    alwaysOn: true
  }
}

resource hostingPlanName 'Microsoft.Web/serverfarms@2018-02-01' = {
  name: appName
  location: location
  kind: ''
  properties: {
    name: appName
    workerSize: workerSize
    workerSizeId: workerSizeId
    numberOfWorkers: numberOfWorkers
  }
  sku: {
    Tier: sku
    Name: skuCode
  }
}

resource app_insights 'microsoft.insights/components@2015-05-01' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

resource kv 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = { 
  name: keyVaultName
}

// Assign secret user permissions to the Azure Function app
var keyVaultSecretsUserRole = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
resource kvFunctionAppPermissions 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(kv.id, app.name, keyVaultSecretsUserRole)
  scope: kv
  properties: {
    principalId: app.identity.principalId
    principalType: 'ServicePrincipal'
    roleDefinitionId: keyVaultSecretsUserRole
  }
}

output miObjectId string = reference(app.id, '2019-08-01', 'full').identity.principalId
