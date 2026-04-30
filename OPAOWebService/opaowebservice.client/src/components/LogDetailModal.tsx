import type { LogEntry } from "../types/LogEntry";

export const LogDetailModal = ({ log, onClose }: { log: LogEntry; onClose: () => void }) => (
    <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000 }}>
        <div style={{ backgroundColor: 'white', padding: '20px', borderRadius: '8px', maxWidth: '800px', width: '90%', maxHeight: '80vh', overflowY: 'auto' }}>
            <h2>Error Details</h2>
            <p><strong>Parcel:</strong> {log.parcelId}</p>
            <p><strong>Exception:</strong> {log.exception}</p>
            <hr />
            <pre style={{ backgroundColor: '#f4f4f4', padding: '15px', borderRadius: '4px', overflowX: 'auto', fontSize: '12px' }}>
                {log.stacktrace || "No stacktrace available."}
            </pre>
            <button onClick={onClose} style={{ marginTop: '10px', padding: '10px 20px', cursor: 'pointer' }}>Close</button>
        </div>
    </div>
);
