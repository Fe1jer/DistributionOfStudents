function HomePage() {
    return <div className="text-center">
        <h1 className="display-4">Приёмная комиссия</h1>
        <p>Здесь будет размещенная информация о приёмной комиссии.</p>
    </div>
;
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<HomePage />);