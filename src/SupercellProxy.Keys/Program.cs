using SupercellProxy.Keys;

int exitCode = await Application.RunAsync(args).ConfigureAwait(continueOnCapturedContext: false);

return exitCode;
