param location string

param appName string
param keyVaultName string


// appservice-web-app
module app 'appservice-web-app.bicep' = {
  name: 'appDeploy'
  params: {
    location: location
    appName: appName
    keyVaultName: keyVaultName
  }
}
