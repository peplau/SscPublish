# SscPublish
Publish extension to Sitecore.Services.Client - A Web API method to securely trigger a Sitecore publish of an item or tree

## Installation
1. Download the package for your corresponding Sitecore version [from here](https://github.com/peplau/SscPublish/tree/master/Download "from here");
1. Install with Installation Wizard

### Post-installation:
Add an entry to ConnectionStrings.config with a SecurityToken to your SSC client:

`<name="Sitecore.Services.Token.SecurityKey" connectionString="key=bHN81iFRbluzXGkNNLRqGAlCytp2w9fm"/>`

Please, don't use this secret - instead, create a strong password (Eg: using https://passwordsgenerator.net/)

## Calling the SSC method

Follow the pattern: 

`/sitecore/api/ssc/SscPublish.Controllers/Publish/{itemId}?{parameters}`

- **ItemId** = your item ID (without brackets)
- **parameters** = Querystring parameters (Eg: ?async=true&deep=false)

## Parameters
- **deep** (bool) - Deep publish
Default: false
- **async** (bool) - Async publish
Default: false
- **targetDb** (string) - Target Database
Default: "web"
- **mode** (string) - Publish Mode
Default: "SingleItem"

### Example calls
- Publish with default values: http://yourserver/sitecore/api/ssc/SscPublish.Controllers/Publish/110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9/
- Deep and Async publish: http://yourserver/sitecore/api/ssc/SscPublish.Controllers/Publish/110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9/?deep=true&async=true

### Available Publish Modes
The implementation will cast the string passed at the **mode** parameter into the enum Sitecore.Publishing.PublishMode

Accepted values are:
- **Full** - Re-publishes everything
- **Incremental** - Every item change generates a record in list (f.e. PublishingQueue table). List is inspected during publishing
- **SingleItem** - Publishes single item
- **Smart** - Publishes items that have difference in source and target
- **Unknown** - Unknown mode
