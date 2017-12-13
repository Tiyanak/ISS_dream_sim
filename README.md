# MonetaryIncentiveDelay
A project for Interactive Simulation Systems university course.

Prvo pokrenuti server, zatim klijent ako se zeli testirati C# konzolna aplikacija.

Metode MsgListener() sluze za slusanje nadoolazezih poruka, kada ce se prebacivati u unity,
potrebno je maknuti vanjsku while petlju i izvrsavati metodu u FixedUpdate() { MsgListener(); }

Listener slusa tipove poruka: Data -> 
	- Data: podaci koji se rucno salju klijentu/serveru sa SendMsg(string)/SendMsg(string, NetConnection) metodom
	Klijent: 
		- OnConnected: kada klijent pozove client.Connect -> dobiva poruku od servera i te zavrsi u switch -> case OnConnected
		- OnDisconnected: kada klijent pozove client.Disconnect -> dobiva poruku od servera i te zavrsi u switch -> case OnDisconnected
	Server: 
		- OnConnected: kada se klijent poveze na server -> witch -> case OnConnected
		- OnDisconnected: kada klijent se odspoji sa servera -> switch -> case OnDisconnected
	- Undefined: nepoznate poruke
	
Razmjenjuju se tipovi poruka: 
	- NetIncomingMessage -> MsgListener prima ulaznu poruku
	- NetOutgoingMessage -> SendMsg salje izlaznu poruku

Server mora slati poruku na klijentsku konekciju: SendMsg(string, NetConnection) {}, koju moze dobiti u MsgListener-u sa: msg.SenderConnection	
	
Import Lidgren-a u unity -> https://github.com/lidgren/lidgren-network-gen3/wiki/Unity3D
