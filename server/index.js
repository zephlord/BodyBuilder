

var express = require('express'); // This makes REST easy

	var port = 3030;
	var app = express();
	


	// allow CORS
	app.all('/*', function(req, res, next) {
	  // CORS headers
	  res.header("Access-Control-Allow-Origin", "*"); // restrict it to the required domain
	  res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE,OPTIONS');
	  // Set custom headers for CORS
	  res.header('Access-Control-Allow-Headers', 'Content-type,Accept,X-Access-Token,X-Key');
	  if (req.method == 'OPTIONS') {
	    res.status(200).end();
	  } else {
	    next();
	  }
	});

	var server = require('http').createServer(app);
	var io = require('socket.io')(server);
  
	var morgan = require('morgan');
	var bodyParser = require('body-parser');
	// configure app to use bodyParser()
	// this will let us get the data from a POST
	app.use(bodyParser.urlencoded({ extended: true }));
	app.use(bodyParser.json());


	require('./router/index')(app, io);

	server.listen(port);
	console.log('server listening at port ' + port);

