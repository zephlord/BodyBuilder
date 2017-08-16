
var crypto = require('crypto'),
    awsSecret = require('../data/awsSecret'),
    bucket = require('../data/s3Bucket');


module.exports = function(key, fileType)
{
    var expiration = new Date(new Date().getTime() + 1000 * 60 * 5).toISOString();
            
    var policy =
    { "expiration": expiration,
        "conditions": [
            {"bucket": bucket},
            {"key": key},
            {"acl": 'public-read'},
            {"Content-Type": fileType},
            ["content-length-range", 0, 524288000]
        ]};

    policyBase64 = new Buffer(JSON.stringify(policy), 'utf8').toString('base64');
    signature = crypto.createHmac('sha1', awsSecret).update(policyBase64).digest('base64');

    return {
        policy: policyBase64,
        signature: signature};
};