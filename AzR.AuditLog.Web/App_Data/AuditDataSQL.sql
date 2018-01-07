TRUNCATE TABLE AuditLogs 
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] ON 

GO
INSERT [dbo].[AuditLogs] ([Id],[LoginId], [KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (1,1, 8, 1, CAST(N'2015-08-16 23:58:29.723' AS DateTime), N'SampleDataModel', N'[{"FieldName":"Id","ValueBefore":"0","ValueAfter":"8"},{"FieldName":"FirstName","ValueBefore":"(null)","ValueAfter":"Adam"},{"FieldName":"LastName","ValueBefore":"(null)","ValueAfter":"Smith"},{"FieldName":"DateOfBirth","ValueBefore":"01/01/0001 00:00:00","ValueAfter":"16/08/1990 00:00:00"}]', N'{"Id":0,"FirstName":null,"LastName":null,"DateOfBirth":"0001-01-01T00:00:00","Deleted":false}', N'{"Id":8,"FirstName":"Adam","LastName":"Smith","DateOfBirth":"1990-08-16T00:00:00","Deleted":false}')
GO
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (2,1, 1, 1, CAST(N'2015-08-16 23:58:56.023' AS DateTime), N'Sample', N'[{"FieldName":"Id","ValueBefore":"0","ValueAfter":"9"},{"FieldName":"FirstName","ValueBefore":"(null)","ValueAfter":"Govindar"},{"FieldName":"LastName","ValueBefore":"(null)","ValueAfter":"Singh"},{"FieldName":"DateOfBirth","ValueBefore":"01/01/0001 00:00:00","ValueAfter":"16/08/1985 00:00:00"}]', N'{"Id":0,"FirstName":null,"LastName":null,"DateOfBirth":"0001-01-01T00:00:00","Deleted":false}', N'{"Id":9,"FirstName":"Govindar","LastName":"Singh","DateOfBirth":"1985-08-16T00:00:00","Deleted":false}')
GO							 
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (3,1, 4, 1, CAST(N'2015-08-16 23:59:24.870' AS DateTime), N'Sample', N'[{"FieldName":"Id","ValueBefore":"0","ValueAfter":"10"},{"FieldName":"FirstName","ValueBefore":"(null)","ValueAfter":"Joe"},{"FieldName":"LastName","ValueBefore":"(null)","ValueAfter":"Public"},{"FieldName":"DateOfBirth","ValueBefore":"01/01/0001 00:00:00","ValueAfter":"21/12/1971 00:00:00"}]', N'{"Id":0,"FirstName":null,"LastName":null,"DateOfBirth":"0001-01-01T00:00:00","Deleted":false}', N'{"Id":10,"FirstName":"Joe","LastName":"Public","DateOfBirth":"1971-12-21T00:00:00","Deleted":false}')
GO							   
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (4,1, 2, 2, CAST(N'2015-08-16 23:59:49.007' AS DateTime), N'Sample', N'[{"FieldName":"DateOfBirth","ValueBefore":"16/08/1990 00:00:00","ValueAfter":"12/04/1989 00:00:00"}]', N'{"Id":8,"FirstName":"Adam","lastname":"Smith","DateOfBirth":"1990-08-16T00:00:00","Deleted":false}', N'{"Id":8,"FirstName":"Adam","lastname":"Smith","DateOfBirth":"1989-04-12T00:00:00","Deleted":false}')
GO							 
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (5,1, 4, 1, CAST(N'2015-08-17 15:50:15.510' AS DateTime), N'Sample', N'[{"FieldName":"Id","ValueBefore":"0","ValueAfter":"11"},{"FieldName":"FirstName","ValueBefore":"(null)","ValueAfter":"Fred"},{"FieldName":"LastName","ValueBefore":"(null)","ValueAfter":"Smith"},{"FieldName":"DateOfBirth","ValueBefore":"01/01/0001 00:00:00","ValueAfter":"17/08/1990 00:00:00"}]', N'{"Id":0,"FirstName":null,"LastName":null,"DateOfBirth":"0001-01-01T00:00:00","Deleted":false}', N'{"Id":11,"FirstName":"Fred","LastName":"Smith","DateOfBirth":"1990-08-17T00:00:00","Deleted":false}')
GO							  
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (6,1, 3, 3, CAST(N'2015-08-17 16:01:22.287' AS DateTime), N'Sample', N'[{"FieldName":"Id","ValueBefore":"10","ValueAfter":"0"},{"FieldName":"FirstName","ValueBefore":"Joe","ValueAfter":"(null)"},{"FieldName":"LastName","ValueBefore":"Public","ValueAfter":"(null)"},{"FieldName":"DateOfBirth","ValueBefore":"21/12/1971 00:00:00","ValueAfter":"01/01/0001 00:00:00"},{"FieldName":"Deleted","ValueBefore":"True","ValueAfter":"False"}]', N'{"Id":10,"FirstName":"Joe","LastName":"Public","DateOfBirth":"1971-12-21T00:00:00","Deleted":true}', N'{"Id":0,"FirstName":null,"LastName":null,"DateOfBirth":"0001-01-01T00:00:00","Deleted":false}')
GO							   
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (7,1, 2, 3, CAST(N'2015-08-17 16:54:54.520' AS DateTime), N'Sample', N'[{"FieldName":"Id","ValueBefore":"8","ValueAfter":"0"},{"FieldName":"FirstName","ValueBefore":"Adam","ValueAfter":"(null)"},{"FieldName":"LastName","ValueBefore":"Smith","ValueAfter":"(null)"},{"FieldName":"DateOfBirth","ValueBefore":"12/04/1989 00:00:00","ValueAfter":"01/01/0001 00:00:00"},{"FieldName":"Deleted","ValueBefore":"True","ValueAfter":"False"}]', N'{"Id":8,"FirstName":"Adam","LastName":"Smith","DateOfBirth":"1989-04-12T00:00:00","Deleted":true}', N'{"Id":0,"FirstName":null,"LastName":null,"DateOfBirth":"0001-01-01T00:00:00","Deleted":false}')
GO							   
INSERT [dbo].[AuditLogs] ([Id],[LoginId],[KeyFieldId], [ActionType], [ActionTime], [EntityName], [ValueChange], [ValueBefore], [ValueAfter]) VALUES (8,1, 1, 2, CAST(N'2015-08-17 17:22:58.173' AS DateTime), N'Sample', N'[]', N'{"Id":9,"FirstName":"Govindar","lastname":"Singh","DateOfBirth":"1985-08-16T00:00:00","Deleted":false}', N'{"Id":9,"FirstName":"Govindar","lastname":"Singh","DateOfBirth":"1985-08-16T00:00:00","Deleted":false}')
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] OFF
GO

TRUNCATE TABLE Samples
GO
SET IDENTITY_INSERT [dbo].[Samples] ON 

GO

INSERT [dbo].[Samples] ([Id], [FirstName], [LastName], [DateOfBirth], [Deleted]) VALUES (1, N'Adam', N'Smith', CAST(N'1989-04-12' AS Date), 1)
GO
INSERT [dbo].[Samples] ([Id], [FirstName], [LastName], [DateOfBirth], [Deleted]) VALUES (2, N'Govindar', N'Singh', CAST(N'1985-08-16' AS Date), 0)
GO
INSERT [dbo].[Samples] ([Id], [FirstName], [LastName], [DateOfBirth], [Deleted]) VALUES (3, N'Joe', N'Public', CAST(N'1971-12-21' AS Date), 1)
GO
INSERT [dbo].[Samples] ([Id], [FirstName], [LastName], [DateOfBirth], [Deleted]) VALUES (4, N'Fred', N'Smith', CAST(N'1990-08-17' AS Date), 0)
GO
SET IDENTITY_INSERT [dbo].[Samples] OFF
GO
