USE [CuentasPagarBD]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 07/02/2018 07:44:52 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[login] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[es_proveedor] [bit] NULL,
	[codigoProveedor] [varchar](50) NULL,
	[razonSocial] [varchar](100) NULL,
	[esSuperUsuario] [bit] NULL,
	[bloqueado] [char](1) NULL,
	[nroIntentos] [int] NULL,
	[cambiarClave] [bit] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 

GO
INSERT [dbo].[Usuario] ([id], [login], [password], [es_proveedor], [codigoProveedor], [razonSocial], [esSuperUsuario], [bloqueado], [nroIntentos], [cambiarClave]) VALUES (1, N'JOEL', N'123456', 0, N'0021030123', NULL, 1, N'0', 0, 0)
GO
INSERT [dbo].[Usuario] ([id], [login], [password], [es_proveedor], [codigoProveedor], [razonSocial], [esSuperUsuario], [bloqueado], [nroIntentos], [cambiarClave]) VALUES (2, N'PROVEEDOR_INT', N'987654', 0, N'', NULL, 0, N'0', 0, 0)
GO
INSERT [dbo].[Usuario] ([id], [login], [password], [es_proveedor], [codigoProveedor], [razonSocial], [esSuperUsuario], [bloqueado], [nroIntentos], [cambiarClave]) VALUES (3, N'DAVID', N'1234', 1, N'0021030123', NULL, 0, N'0', 0, 0)
GO
INSERT [dbo].[Usuario] ([id], [login], [password], [es_proveedor], [codigoProveedor], [razonSocial], [esSuperUsuario], [bloqueado], [nroIntentos], [cambiarClave]) VALUES (4, N'LIGIA.PUERTAS', N'Lcpj1980', 1, N'0021030123', N'Polar CA', 1, N'0', 0, 0)
GO
INSERT [dbo].[Usuario] ([id], [login], [password], [es_proveedor], [codigoProveedor], [razonSocial], [esSuperUsuario], [bloqueado], [nroIntentos], [cambiarClave]) VALUES (5, N'MAURICIO', N'123', 1, N'0021030123', N'Empresa Prueba', 0, N'0', 0, 0)
GO
INSERT [dbo].[Usuario] ([id], [login], [password], [es_proveedor], [codigoProveedor], [razonSocial], [esSuperUsuario], [bloqueado], [nroIntentos], [cambiarClave]) VALUES (6, N'PROVEEDOR_EXT', N'147258', 1, N'0021030123', N'Empresa MX', 0, N'0', 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_es_proveedor]  DEFAULT ((0)) FOR [es_proveedor]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_esSuperUsuario]  DEFAULT ((0)) FOR [esSuperUsuario]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_estatusBloqueo]  DEFAULT ((1)) FOR [bloqueado]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_nroIntentosBloqueo]  DEFAULT ((0)) FOR [nroIntentos]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_estatusCambioClave]  DEFAULT ((0)) FOR [cambiarClave]
GO
