using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Capture;

internal sealed record CapturedLoginExchange(LoginMessage Login, IMessage Outcome, OwnHomeDataMessage OwnHomeData);
