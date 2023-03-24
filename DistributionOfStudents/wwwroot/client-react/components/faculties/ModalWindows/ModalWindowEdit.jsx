export default function ModalWindowEdit({ onEdit }) {
    const [shortName, setShortName] = React.useState("");

    const onSubmit = (e) => {
        e.preventDefault();
        onEdit(shortName);
    }
    React.useEffect(() => {
        var exampleModal = document.getElementById('facultyEditModalWindow')
        exampleModal.addEventListener('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = event.relatedTarget;
            // Extract info from data-bs-* attributes
            setShortName(button.getAttribute('data-bs-shortname'));
        })
    }, [])
    return (
        <form onSubmit={onSubmit}>
            <div className="modal fade" id="facultyEditModalWindow" tabIndex="-1" role="dialog" aria-modal="true" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Сохранить изменения</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <p>Перед тем как изменить данные факультета, проверьте заполнение всех ячеек описания факультета.</p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Закрыть</button>
                            <input type="submit" className="btn btn-primary" value="Сохранить" />
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
