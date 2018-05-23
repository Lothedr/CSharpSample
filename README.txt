Copyright (C) Stanislaw Piotrowski <stanislaw.piotrowski717@gmail.com> - All Rights Reserved

This is sample project made in .NET Core. The project includes simple client and
server with chat room functionality. TCP is used as a communication protocol.

To launch the server, run:
dotnet Server.dll [port]
If a port is not provided or invalid then port 8500 will be chosen.

To launch the client, run:
dotnet Client.dll [port [address]]
If a port is not provided or invalid then port 8500 will be chosen.
If an address is not provided or invalid then address 127.0.0.1 will be chosen.

Clients must register on the server with an unused name to be able to switch rooms.
Type /register NAME to register with the provided name.
After registration, type /join ROOMNAME to join a room.
After joining the room, type anything which does not start with '/' to chat.
Type /exit to quit.

To close server, press Q.

