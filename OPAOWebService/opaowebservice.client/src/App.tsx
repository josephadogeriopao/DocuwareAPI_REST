import { useState, useEffect, useMemo } from "react";
import { LogDetailModal } from "./components/LogDetail.";
import { LogTable } from "./components/LogTable";
import { Pagination } from "./components/Pagination";
import type { LogEntry } from "./types/LogEntry";
import { LogSidebar } from "./components/LogSideBar";

function App() {
    const [logEntries, setLogEntries] = useState<LogEntry[]>([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [selectedLog, setSelectedLog] = useState<LogEntry | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 10;

    // 1. Fetching
    useEffect(() => {
        fetch('/log/logentries').then(res => res.json()).then(setLogEntries);
    }, []);

    // 2. Filtering (Dates + Search)
    const filteredLogs = useMemo(() => {
        return logEntries.filter(log => {
            const matchesSearch = log.parcelId?.toLowerCase().includes(searchTerm.toLowerCase());
            const logTime = new Date(log.datetime).getTime();
            const start = fromDate ? new Date(fromDate).getTime() : -Infinity;
            const end = toDate ? new Date(toDate).getTime() : Infinity;
            return matchesSearch && logTime >= start && logTime <= end;
        });
    }, [logEntries, fromDate, toDate, searchTerm]);

    const currentLogs = filteredLogs.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

    return (
        <div style={{ display: 'flex', gap: '20px', padding: '20px', fontFamily: 'Arial' }}>
            <LogSidebar
                fromDate={fromDate} toDate={toDate}
                onFromChange={setFromDate} onToChange={setToDate}
                onClear={() => { setFromDate(''); setToDate(''); setSearchTerm(''); }}
            >
                {/* Extra search input in sidebar */}
                <div style={{ marginTop: '15px' }}>
                    <label>Parcel ID:</label>
                    <input type="text" placeholder="Search..." value={searchTerm} onChange={e => setSearchTerm(e.target.value)} />
                </div>
            </LogSidebar>

            <main style={{ flex: 1 }}>
                <h1>OPAO Web Service Logs</h1>
                <LogTable logs={currentLogs} onSelect={setSelectedLog} />
                <Pagination total={Math.ceil(filteredLogs.length / itemsPerPage)} current={currentPage} onChange={setCurrentPage} />
            </main>

            {selectedLog && <LogDetailModal log={selectedLog} onClose={() => setSelectedLog(null)} />}
        </div>
    );
}

export default App;