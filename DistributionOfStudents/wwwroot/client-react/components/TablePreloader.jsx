export default function TablePreloader() {
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0]
    return (
        <div className="card shadow">
            <table className="table mb-0">
                <thead>
                    <tr>
                        <th><p className="placeholder-glow"><span className="placeholder w-50"></span></p></th>
                        <th><p className="placeholder-glow"><span className="placeholder w-50"></span></p></th>
                        <th><p className="placeholder-glow"><span className="placeholder w-50"></span></p></th>
                    </tr>
                </thead>
                <tbody>
                    {numbers.map((number) =>
                        <tr key={number} className="align-middle">
                            <td><p className="placeholder-glow"><span className="placeholder w-75"></span></p></td>
                            <td><p className="placeholder-glow"><span className="placeholder w-75"></span></p></td>
                            <td><p className="placeholder-glow"><span className="placeholder w-75"></span></p></td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}