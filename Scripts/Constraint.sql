CREATE FUNCTION CheckUniqueCodigoProveedor(
  @CodigoProveedor VARCHAR(50)
) RETURNS INT AS BEGIN

  DECLARE @ret INT;
  SELECT @ret = COUNT(*) FROM dbo.Usuario WHERE codigoProveedor = @CodigoProveedor AND LEN(codigoProveedor) > 0;
  RETURN @ret;

END;
GO

ALTER TABLE [dbo].[Usuario]
  ADD CONSTRAINT [UC_Usuario_codigoProveedor]  CHECK (NOT (dbo.CheckUniqueCodigoProveedor(codigoProveedor) > 1 AND LEN(codigoProveedor) > 0));
GO

ALTER TABLE [dbo].[Usuario]
  ADD UNIQUE (login);
GO

$@PAXXIB2OIT