/**
 * Represents a log entry record retrieved from the OPAOWebService API.
 * 
 * @author Joseph Adogeri
 * @since 30-APR-2026
 * @version 1.0.0
 */
export type LogEntry = {
    /** Unique Millisecond Timestamp ID */
    id: number;

    /** Date and Time of the log event (ISO 8601 string) */
    datetime: string;

    /** Associated Parcel Identifier */
    parcelId: string | null;

    /** Application Tier (e.g., Presentation, Infrastructure) */
    tier: string | null;

    /** Short Exception Name (e.g., ParcelLockedException) */
    exception: string | null;

    /** Human-readable message description */
    description: string | null;

    /** Technical Stack Trace for debugging */
    stacktrace: string | null;
};
