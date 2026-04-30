type PaginationProps = {
    total: number;
    current: number;
    onChange: (page: number) => void;
};

export const Pagination = ({ total, current, onChange }: PaginationProps) => (
    <div style={{ marginTop: '20px', display: 'flex', gap: '5px' }}>
        {Array.from({ length: total }, (_, i) => (
            <button
                key={i + 1}
                onClick={() => onChange(i + 1)}
                style={{
                    padding: '5px 10px',
                    backgroundColor: current === i + 1 ? '#007bff' : '#fff',
                    color: current === i + 1 ? '#fff' : '#000'
                }}
            >
                {i + 1}
            </button>
        ))}
    </div>
);
