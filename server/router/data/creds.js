var AWS = require('aws-sdk'),
    awsKey = require('./awsKey'),
    awsSecret = require('./awsSecret');

    function credentials()
    {
        return new AWS.Credentials(awsKey, awsSecret);
    }

module.exports = new credentials();