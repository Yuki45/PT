#pragma once

//-------------------------Constants------------------------------------------ 

#define	MAX_SLAVE_NUMS				(16)

#define	MAX_BUFFER_SIZE				(256)		///< Maximum Send/Receive Packet Size
#define	MAX_SWINFO_LENGTH			(248)

#define	COMM_WAITTIME				(100)

#define	COMM_TIME_STATUSREFRESH		(100)

#define	COMM_TIME_IDLE				(/*COMM_TIME_WAITING*/500 / COMM_TIME_STATUSREFRESH)

#define	MAX_ALLOWED_TIMEOUT			(6)