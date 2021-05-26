--TABLE SCRIPTS for DEV and TEST 2/23/21
--===============================================
-->DEV
--===============================================
/*********************************************
event
*********************************************/
CREATE TABLE [dev].[event](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_event] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dev].[event] ADD  CONSTRAINT [DF_event_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
shop
*********************************************/
CREATE TABLE [dev].[shop](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_shop] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dev].[shop] ADD  CONSTRAINT [DF_shop_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
user
*********************************************/
CREATE TABLE [dev].[user](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dev].[user] ADD  CONSTRAINT [DF_user_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
product
*********************************************/
CREATE TABLE [dev].[product](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_product] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dev].[product] ADD  CONSTRAINT [DF_product_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
request log
*********************************************/
CREATE TABLE [dev].[request_log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NULL,
	[Parameters] [varchar](max) NULL,
	[Route] [varchar](1000) NULL,
 CONSTRAINT [PK_request_log] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--===============================================
-->TEST
--===============================================
/*********************************************
event
*********************************************/
CREATE TABLE [test].[event](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_event] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [test].[event] ADD  CONSTRAINT [DF_event_date_created]  DEFAULT (getdate()) FOR [date_created]
GO
/*********************************************
shop
*********************************************/
CREATE TABLE [test].[shop](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_shop] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [test].[shop] ADD  CONSTRAINT [DF_shop_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
user
*********************************************/
CREATE TABLE [test].[user](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [test].[user] ADD  CONSTRAINT [DF_user_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
product
*********************************************/
CREATE TABLE [test].[product](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[id] [varchar](50) NOT NULL,
	[data] [varchar](max) NULL,
	[date_created] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [PK_product] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [test].[product] ADD  CONSTRAINT [DF_product_date_created]  DEFAULT (getdate()) FOR [date_created]
GO

/*********************************************
request log
*********************************************/
CREATE TABLE [test].[request_log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NULL,
	[Parameters] [varchar](max) NULL,
	[Route] [varchar](1000) NULL,
 CONSTRAINT [PK_request_log] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO