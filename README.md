# Indeximals

Indexing animals with blob storage

## Provision

You need a storage account and a search account in Azure.
You can create both of these with zero cost :)

## Add secrets

Drop your secrets into a file in the root of the project called secrets.json

```
{
    "StorageConnectionString": "DefaultEndpointsProtocol=https;AccountName=XXXXX;AccountKey=XXXXXXX;EndpointSuffix=core.windows.net"
}
```

## Create some data

Navigate to the 'Create' page where you will see a form.
Each form you submit will update a blob storage.
The blob will be named <Id>.json and will be placed in a container called 'animals'

## Connect storage to search

```
POST https://XXXXXX.search.windows.net/datasources?api-version=2016-09-01
Content-Type: application/json
api-key: XXXXXXXXXXXXX

{
    "name" : "my-blob-datasource",
    "type" : "azureblob",
    "credentials" : { "connectionString" : "DefaultEndpointsProtocol=https;AccountName=XXXXXXX;AccountKey=XXXXXXXXXXXX;" },
    "container" : { "name" : "animals" }
}   
```

Create an index in search to define schema that matches your blobs (portal or API)
Minimum schedule for indexing is 5 minutes

```
POST https://XXXXXXXXXX.search.windows.net/indexers?api-version=2016-09-01
Content-Type: application/json
api-key: XXXXXXXXXXXXX

{
  "name" : "my-json-indexer",
  "dataSourceName" : "my-blob-datasource",
  "targetIndexName" : "index",
  "schedule" : { "interval" : "PT10M" },
  "parameters" : { "configuration" : { "parsingMode" : "json" } }
}
```

You now have an existing data source for your search index you can see it in the portal.
