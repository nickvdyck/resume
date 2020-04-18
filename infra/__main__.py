from pulumi import Config, export
from pulumi_azure import cdn, core, storage

from static import StorageStaticWebsite
from cloudflare import create_dns_record

config = Config(name="resume-infra")

domain = config.require("domain")
zone_id = config.require("zone_id")

resource_group = core.ResourceGroup('nvd-codes-resume')

account = storage.Account('resumestorage',
    resource_group_name=resource_group.name,
    account_tier='Standard',
    account_replication_type='LRS'
)

static_website = StorageStaticWebsite(
    "resume-storage-static", account_name=account.name
)

cdn_profile = cdn.Profile(
    "resume-cdn", resource_group_name=resource_group.name, sku="Standard_Microsoft"
)

endpoint = cdn.Endpoint(
    "resume-cdn-ep",
    resource_group_name=resource_group.name,
    profile_name=cdn_profile.name,
    origin_host_header=static_website.hostname,
    origins=[{"name": "blobstorage", "hostName": static_website.hostname}],
)

dns_record = create_dns_record(
    "resume", domain, zone_id, endpoint.host_name, "cdnverify"
)

export('connection_string', account.primary_connection_string)
