SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cliente](
	[idCliente] [int] NOT NULL,
	[cedula] [nvarchar](20) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[apellidos] [nvarchar](80) NOT NULL,
	[telefono] [int] NOT NULL,
	[correoElectronico] [nvarchar](50) NOT NULL,
	[direccion] [nvarchar](100) NOT NULL,
	[fotoCedula] [nvarchar](150) NOT NULL,
	[fechaNacimiento] [date] NOT NULL,
	[fotoPerfil] [nvarchar](100) NULL,
	[contrasena] [nvarchar](20) NOT NULL,
	[IBAN] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[idCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO