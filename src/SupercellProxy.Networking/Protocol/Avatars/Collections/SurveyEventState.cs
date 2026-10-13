namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>A survey event's impression and state, saved natively as <c>survey_event_states_v2</c>.</summary>
public readonly record struct SurveyEventState(int EventId, int ImpressionId, int State);
