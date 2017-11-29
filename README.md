# Indeximals

Indexing animals with blob storage

## Connect storage to search

```
POST https://indeximals.search.windows.net/datasources?api-version=2016-09-01
Content-Type: application/json
api-key: XXXXXXXXXXXXX

{
    "name" : "my-blob-datasource",
    "type" : "azureblob",
    "credentials" : { "connectionString" : "DefaultEndpointsProtocol=https;AccountName=indeximals;AccountKey=XXXXXXXXXXXX;" },
    "container" : { "name" : "animals" }
}   
```

Create an index in search to define schema that matches your blobs (portal or API)
Minimum schedule for indexing is 5 minutes

```
POST https://indeximals.search.windows.net/indexers?api-version=2016-09-01
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
