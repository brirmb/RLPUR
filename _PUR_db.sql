

INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'Purchase', N'iiWeb', N'请购作业', NULL, NULL, 1, N'A', N'Admin', getdate())
GO
INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'CommonPur', N'Purchase', N'一般请购', NULL, N'Web/CommonPur.aspx', 1, N'A', N'Admin', getdate())
GO
INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'OutsidePur', N'Purchase', N'委外请购', NULL, N'Web/OutsidePur.aspx', 2, N'A', N'Admin', getdate())
GO
INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'MaterialPur', N'Purchase', N'材料请购', NULL, N'Web/MaterialPur.aspx', 3, N'A', N'Admin', getdate())
GO

INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'PurchaseB', N'iiWeb', N'采购作业', NULL, NULL, 1, N'A', N'Admin', getdate())
GO
INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'PurMaintain', N'PurchaseB', N'请购维护', NULL, N'Web/PurMaintain.aspx', 1, N'A', N'Admin', getdate())
GO

INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'Shipment', N'iiWeb', N'出货作业', NULL, NULL, 1, N'A', N'Admin', getdate())
GO
INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'ShipNotice', N'Shipment', N'出货通知单', NULL, N'Web/ShipNotice.aspx', 1, N'A', N'Admin', getdate())
GO
INSERT [dbo].[iiWeb_Module] ([ID], [PID], [Name], [Remark], [URL], [SEQ], [Status], [MUser], [MTime]) VALUES (N'ShipMaintain', N'Shipment', N'出货', NULL, N'Web/ShipMaintain.aspx', 1, N'A', N'Admin', getdate())
GO