var s3 = require('../data/S3Object'),
    manifestName = require('../data/manifestName'),
    q = require('q');

module.exports = function(data, bucket) {
    var deferObj = q.defer();

    var completePut = function(err, result) {
        if (err)
            deferObj.reject(err);
        else
            deferObj.resolve();
    };


    s3.putString(manifestName, JSON.stringify(data), bucket, completePut);

    return deferObj.promise;
};