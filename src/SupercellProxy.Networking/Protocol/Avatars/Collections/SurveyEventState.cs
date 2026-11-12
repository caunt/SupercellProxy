namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>A survey event's impression and state, saved natively as <c>survey_event_states_v2</c>.</summary>
/// <param name="EventId">The live event id of the survey event, matching <see cref="LiveEvent.EventId"/>; written as a VarInt.</param>
/// <param name="ImpressionId">The impression id of the survey showing this state belongs to, matching <see cref="LiveEvent.ImpressionId"/> of a targeted event; written as a VarInt.</param>
/// <param name="State">The survey's progress state as a VarInt; the meaning of the individual values is not yet identified.</param>
public readonly record struct SurveyEventState(int EventId, int ImpressionId, int State);
