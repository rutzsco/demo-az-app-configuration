param appName string
param location string = resourceGroup().location
param sku string = 'B1'

param environment string = 'Production'
param applicationInsightsName string = appName
param appConfigurationConnectionString string

resource app 'Microsoft.Web/sites@2022-03-01' = {
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
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
        {
          name: 'ConnectionStrings::AppConfig'
          value: appConfigurationConnectionString
        }
      ]
    }
    serverFarmId: hostingPlanName.id
    clientAffinityEnabled: true
    httpsOnly: true
  }
}

resource hostingPlanName 'Microsoft.Web/serverfarms@2018-02-01' = {
  name: appName
  location: location
  sku: {
    name: sku
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

output miObjectId string = reference(app.id, '2019-08-01', 'full').identity.principalId
