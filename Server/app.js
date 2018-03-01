var io = require('socket.io')(process.envPort||3000);
var shortid = require('shortid');
var mongoClient = require('mongodb');

var url = "mongodb://localhost:27017/";
var dbName = "GameData";
var db;
mongoClient.connect(url, function(error, client)
{
	if( error ) { throw error; }
	
	console.log("Connected to MongoDB");
	db = client.db(dbName);
});

console.log("Server Started");

var players = [];

io.on('connection', function(socket)
{
	var thisPlayerId = shortid.generate();
	
	players.push(thisPlayerId);
	
	console.log('client connected spawning player id: '+ thisPlayerId);
	
	socket.broadcast.emit('spawn player', {id:thisPlayerId});
	
	players.forEach(function(playerId)
	{
		if(playerId == thisPlayerId)return;
		
		socket.emit('spawn player', {id:playerId});
		console.log("Adding a new player", playerId);
	});
	
	socket.on('move', function(data)
	{
		data.id = thisPlayerId;
		
		socket.broadcast.emit("move", data);
	});
	
	socket.on('disconnect', function()
	{
		console.log("Player Disconnected");
		players.splice(players.indexOf(thisPlayerId),1);
		socket.broadcast.emit('disconnected', {id:thisPlayerId});
	});
	
	socket.on("sent gameData", function(data)
	{
		
		db.collection("playerData").save(data, function(error, result)
		{
			if( error ) { throw error; }
			
			console.log("Player Data Saved");
		});
	});
});