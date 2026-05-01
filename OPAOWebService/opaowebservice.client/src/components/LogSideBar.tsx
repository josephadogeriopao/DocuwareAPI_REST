import React from 'react'; // Import ReactNode

type SidebarProps = {
    fromDate: string;
    toDate: string;
    onFromChange: (val: string) => void;
    onToChange: (val: string) => void;
    onClear: () => void;
    children?: React.ReactNode; // ADD THIS: Allows passing the Parcel ID input
};

export const LogSidebar = ({
    fromDate,
    toDate,
    onFromChange,
    onToChange,
    onClear,
    children // Destructure children here
}: SidebarProps) => (
    <aside style={{ width: '250px', borderRight: '1px solid #ddd', paddingRight: '20px' }}>
        <h3>Search Filters</h3>
        <div style={{ marginBottom: '15px' }}>
            <label>Date From:</label>
            <input type="date" value={fromDate} onChange={(e) => onFromChange(e.target.value)} style={{ width: '100%' }} />
        </div>
        <div style={{ marginBottom: '15px' }}>
            <label>Date To:</label>
            <input type="date" value={toDate} onChange={(e) => onToChange(e.target.value)} style={{ width: '100%' }} />
        </div>

        {/* Render the Parcel ID search bar here */}
        {children}

        <div style={{ marginTop: '20px' }}>
            <button onClick={onClear} style={{ width: '100%' }}>Clear Filters</button>
        </div>
    </aside>
);
