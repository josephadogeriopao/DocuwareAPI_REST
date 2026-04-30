import type { LogEntry } from "../types/LogEntry";

export const LogTable = ({ logs, onSelect }: { logs: LogEntry[], onSelect: (log: LogEntry) => void }) => (
    <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
            <tr style={{ background: '#f4f4f4', textAlign: 'left' }}>
                <th style={{ padding: '10px', border: '1px solid #ddd' }}>Date Time</th>
                <th style={{ padding: '10px', border: '1px solid #ddd' }}>Parcel ID</th>
                <th style={{ padding: '10px', border: '1px solid #ddd' }}>Exception</th>
                <th style={{ padding: '10px', border: '1px solid #ddd' }}>Action</th>
            </tr>
        </thead>
        <tbody>
            {logs.map(log => (
                <tr key={log.id}>
                    <td style={{ padding: '10px', border: '1px solid #ddd' }}>{log.datetime.split('.')[0].replace('T', ' ')}</td>
                    <td style={{ padding: '10px', border: '1px solid #ddd' }}>{log.parcelId}</td>
                    <td style={{ padding: '10px', border: '1px solid #ddd', color: 'red' }}>{log.exception}</td>
                    <td style={{ padding: '10px', border: '1px solid #ddd' }}>
                        <button onClick={() => onSelect(log)}>View Details</button>
                    </td>
                </tr>
            ))}
        </tbody>
    </table>
);
