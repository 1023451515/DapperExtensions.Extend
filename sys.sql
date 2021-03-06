USE [HY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cnName] [varchar](20) NOT NULL,
	[enName] [varchar](10) NULL,
	[deptID] [int] NOT NULL,
	[pwd] [varchar](32) NULL,
	[sex] [int] NOT NULL,
	[age] [int] NULL,
	[qq] [varchar](15) NULL,
	[mobile] [varchar](11) NULL,
	[email] [varchar](30) NULL,
	[state] [int] NOT NULL,
	[remark] [varchar](50) NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1.男 2.女' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1.正常 2.禁用 3.删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'state'
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroup](
	[uid] [int] NOT NULL,
	[gid] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuOperation]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MenuOperation](
	[menuID] [int] NOT NULL,
	[operationCode] [varchar](10) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MenuInfo]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MenuInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cnName] [varchar](20) NOT NULL,
	[enName] [varchar](10) NULL,
	[pid] [int] NOT NULL,
	[sort] [int] NULL,
	[dllName] [varchar](30) NULL,
	[reflectType] [varchar](50) NULL,
	[state] [int] NOT NULL,
	[remark] [varchar](30) NULL,
 CONSTRAINT [PK_MenuInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1.正常 2.不可见 3.删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuInfo', @level2type=N'COLUMN',@level2name=N'state'
GO
/****** Object:  Table [dbo].[GroupPermission]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupPermission](
	[groupID] [int] NOT NULL,
	[menuID] [int] NOT NULL,
	[operationCode] [varchar](10) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupInfo]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[groupName] [varchar](20) NOT NULL,
	[remark] [varchar](50) NULL,
 CONSTRAINT [PK_GroupInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeptInfo]    Script Date: 11/15/2017 13:53:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeptInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[deptName] [varchar](20) NOT NULL,
	[remark] [varchar](50) NULL,
 CONSTRAINT [PK_DeptInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_MenuInfo_sort]    Script Date: 11/15/2017 13:53:15 ******/
ALTER TABLE [dbo].[MenuInfo] ADD  CONSTRAINT [DF_MenuInfo_sort]  DEFAULT ((1)) FOR [sort]
GO
/****** Object:  Default [DF_MenuInfo_state]    Script Date: 11/15/2017 13:53:15 ******/
ALTER TABLE [dbo].[MenuInfo] ADD  CONSTRAINT [DF_MenuInfo_state]  DEFAULT ((1)) FOR [state]
GO
/****** Object:  Default [DF_UserInfo_sex]    Script Date: 11/15/2017 13:53:15 ******/
ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_sex]  DEFAULT ((1)) FOR [sex]
GO
/****** Object:  Default [DF_UserInfo_state]    Script Date: 11/15/2017 13:53:15 ******/
ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF_UserInfo_state]  DEFAULT ((1)) FOR [state]
GO


--新增数据
INSERT INTO [HY].[dbo].[DeptInfo]([deptName],[remark]) VALUES ('总经办',null);
INSERT INTO [HY].[dbo].[DeptInfo]([deptName],[remark]) VALUES ('行政部',null);
INSERT INTO [HY].[dbo].[DeptInfo]([deptName],[remark]) VALUES ('财务部',null);
INSERT INTO [HY].[dbo].[DeptInfo]([deptName],[remark]) VALUES ('技术部',null);
INSERT INTO [HY].[dbo].[DeptInfo]([deptName],[remark]) VALUES ('市场部',null);
INSERT INTO [HY].[dbo].[DeptInfo]([deptName],[remark]) VALUES ('生产部',null);

INSERT INTO [HY].[dbo].[UserInfo]([cnName],[enName],[deptID],[pwd],[sex],[age],[qq],[mobile],[email],[state],[remark]) VALUES('超级管理员','superAdmin',1,'e10adc3949ba59abbe56e057f20f883e',1,18,'123456','18812345678','123@xx.com',1,'');

