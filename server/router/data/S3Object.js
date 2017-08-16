var AWS = require('aws-sdk'),
    creds = require('./creds'),
    fs = require('fs'),
    s3 = new AWS.S3({ credentials: creds });


function s3Setup() {

    var createBucket = function(bucket, callback) {
        s3.createBucket({ Bucket: bucket }, callback);
    }

    var checkBucketExists = function(bucket, callback) {
        s3.headBucket({ Bucket: bucket }, function(error, data) {
            if (error) {
                if (error.code == 'NotFound')
                    createBucket(bucket, callback);
                else
                    console.log(error.code);
            } else
                callback(error, data);
        });
    }

    var doesFileExist = function(key, bucket, callback) {
        var params = {
            Bucket: bucket,
            Key: key
        };
        s3.headObject(params,
            function(err, data) {
                if (err) {
                    if (err.code == 'NotFound')
                        callback(null, false);
                    else
                        callback(err, null);
                } else
                    callback(null, true);
            });
    }

    this.get = function(key, bucket, callback) {
        s3.getObject({
            Bucket: bucket,
            Key: key
        }, callback);
    };

    this.put = function(key, filePath, type, bucket, callback) {

        checkBucketExists(bucket, function(error, data) {
            if (error)
                callback(error, data);
            else {

                doesFileExist(key, bucket,
                    function(err, exists) {
                        if (exists)
                            callback({
                                code: 'FileExists',
                                message: 'File' + key + ' already exists in bucket ' + bucket +
                                    '. Please rename and try again'
                            }, null);

                        else {
                            var params = {
                                Bucket: bucket,
                                Key: key,
                                ACL: 'public-read',
                                Body: fs.createReadStream(filePath),
                                ServerSideEncryption: 'AES256',
                            }
                            if (type != null)
                                params.ContentType = type;
                            s3.putObject(params, function(er, data) {
                                if (er)
                                    callback(er, data);
                                else {
                                    data.fileName = key;
                                    callback(er, data);
                                }

                            });
                        }
                    });
            }
        });


    };

    this.putFile = function(key, filePath, bucket, callback) {
        this.put(key, filePath, null, bucket, callback);
    };

    this.putString = function(key, data, bucket, callback) {
        s3.putObject({
            Bucket: bucket,
            Key: key,
            Body: data,
            ContentType: 'text/plain',
            ServerSideEncryption: 'AES256',
            ACL: 'public-read'
        }, callback);
    };

    this.getSignedURL = function(key, bucket, callback, animIndex) {
        var params = { Bucket: bucket, Key: key };
        s3.getSignedUrl('getObject', params, function(error, url) {
            var data = {};
            data.url = url;
            data.animIndex = animIndex;
            callback(error, data);
        });
    };

    this.getObject = function(key, bucket, callback) {
        var params = { Bucket: bucket, Key: key };
        s3.getObject(params, callback);
    };

    var appendAllObjectList = function(bucketName, result, list, callback) {
        for (s3obj in result.Contents)
            list.push(result.Contents[s3obj].Key);
        if (!result.IsTruncated)
            callback(null, list);
        else {
            var params = { Bucket: bucketName, ContinuationToken: result.NextContinuationToken };
            s3.listObjectsV2(params, function(err, result) {
                if (err)
                    callback(err, list);
                else
                    appendAllObjectList(bucketName, result, list, callback);
            });
        }

    };

    this.listAllObjects = function(bucket, callback) {
        checkBucketExists(bucket, function(error, data) {
            if (error)
                callback(error, data);
            else {
                var params = { Bucket: bucket }
                s3.listObjectsV2(params, function(err, result) {
                    if (err)
                        callback(err, null);
                    else
                        appendAllObjectList(bucket, result, [], callback);
                });
            }
        });
    }

    this.copyObject = function(fromBucket, toBucket, key, newKey, callback) {
        var params = {
            Bucket: toBucket,
            CopySource: "/" + fromBucket + "/" + key,
            Key: newKey
        };
        s3.copyObject(params, callback);
    }

    this.delete = function(bucketName, key, callback) {
        var params = { Bucket: bucketName, Key: key };
        s3.deleteObject(params, callback);
    }


    return this;
}

module.exports = new s3Setup();