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
