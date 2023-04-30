import Placeholder from 'react-bootstrap/Placeholder';
import Modal from 'react-bootstrap/Modal';

export default function ModalWindowPreloader({ show, handleClose }) {
    return (
        <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Modal.Header closeButton>
                <Placeholder as={Modal.Title} animation="glow" className="w-100"><Placeholder xs={6} /></Placeholder>
            </Modal.Header>
            <Modal.Body className="text-center">
                Загрузка...
            </Modal.Body>
            <Modal.Footer>
                <Placeholder.Button variant="secondary" style={{ width: 80 }} />
                <Placeholder.Button variant="success" style={{ width: 80 }} />
            </Modal.Footer>
        </Modal>
    );
}