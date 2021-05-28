FOR /D /R . %%X IN (.vs) DO RD /S /Q "%%X"
FOR /D /R . %%X IN (packages) DO RD /S /Q "%%X"
FOR /D /R . %%X IN (_ReSharper.*) DO RD /S /Q "%%X"
FOR /D /R . %%X IN (bin) DO RD /S /Q "%%X"
FOR /D /R . %%X IN (obj) DO RD /S /Q "%%X"
FOR /D /R . %%X IN (umbraco) DO RD /S /Q "%%X"
FOR /D /R . %%X IN (umbraco_client) DO RD /S /Q "%%X"
DEL /s /q /f *.suo
DEL /s /q /f *.orig