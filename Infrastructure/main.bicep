param location string
param appName string

// appConfig
resource appConfig 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  location: location
  name: '${appName}-configuration'
  sku: {
    name: 'standard'
  }
  properties: {

  }
}

var readonlyKey = filter(appConfig.listKeys().value, k => k.name == 'Primary Read Only')[0]
output readonly string = readonlyKey.connectionString

// appservice-web-app
module app 'appservice-web-app.bicep' = {
  name: 'appDeploy'
  params: {
    location: location
    appName: appName
  }
}
