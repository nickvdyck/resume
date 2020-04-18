from pulumi import Config, export
from pulumi_azure import cdn, core, storage

from cloudflare import create_dns_record

config = Config(name="resume-infra")

domain = config.require("domain")
zone_id = config.require("zone_id")

resource_group = core.ResourceGroup('nvd-codes-resume')

account = storage.Account('resumestorage',
    resource_group_name=resource_group.name,
    account_tier='Standard',
    account_replication_type='LRS',
    enable_https_traffic_only=False,
    static_website={
        "indexDocument": "index.html",
    }
)

cdn_profile = cdn.Profile(
    "resume-cdn",
    resource_group_name=resource_group.name,
    sku="Standard_Microsoft"
)

endpoint = cdn.Endpoint(
    "resume-cdn-ep",
    resource_group_name=resource_group.name,
    profile_name=cdn_profile.name,
    origin_host_header=account.primary_web_host,
    origins=[{"name": "blobstorage", "hostName": account.primary_web_host}],
)

dns_record = create_dns_record(
    "resume", domain, zone_id, endpoint.host_name, "cdnverify"
)

export('connection_string', account.primary_connection_string)
