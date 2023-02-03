param location string
param appName string

// appservice-web-app
module app 'appservice-web-app.bicep' = {
  name: 'appDeploy'
  params: {
    location: location
    appName: appName
  }
}

resource configurationStore 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  location: location
  name: '${appName}-configuration'
  sku: {
    name: 'standard'
  }
  properties: {

  }
}
