import Modal from 'react-bootstrap/Modal'
import PropTypes from "prop-types";
import {useState, useEffect } from 'react'

const AnswerViewer = (props: any) => {
    const [data, setData] = useState([]);

    async function fetchAnswers() {
        let response = await fetch(`/api/answers/${props.quiz.id}/${props.answer.id}`, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            }
        });
        if (response.ok != true) {
            return;
        }

        let answersCollection = await response.json();
        answersCollection = answersCollection[0]['answers'];
        let newData : any = {};
        answersCollection.forEach((answer : any) => {
            newData[answer.fieldName] = answer.text;
        });
        setData(newData);
    }

    useEffect(() => {
        fetchAnswers();
    }, [props.answer])

    if (!props.answer) {
        return;
    }

    function shortenTime(isoString: string): string {
        if (!/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{1,7}$/.test(isoString)) {
            throw new Error("Invalid ISO 8601 string format");
        }

        const [date, time] = isoString.split('T');
        const [hours, minutes] = time.split(':');
        return `${date} ${hours}:${minutes}`;
    }

    return (
        <Modal className="create-new-item-dialog"
            fullscreen={true}
            show={props.show}
            onHide={props.onHide}
            dialogClassName="modal-90w"
            aria-labelledby="example-custom-modal-styling-title"
        >
            <Modal.Header closeButton>
                <Modal.Title id="example-custom-modal-styling-title">
                    Answers view
                </Modal.Title>
            </Modal.Header>
            <Modal.Body className="body">
                <div className="limiter-container-wrapper">
                    <div className="limiter-container">
                        <div><label>{props.answer.userNickname} {shortenTime(props.answer.answerTime)}</label></div>
                        {data.length != 0 ?
                        (
                            <div style={styles.container}>
                                {Object.entries(data).map(([field, value]) => (
                                    <div key={field} style={styles.field}>
                                        <div style={styles.fieldName}>{field}</div>
                                        <div style={styles.fieldValue}>{value}</div>
                                    </div>
                                ))}
                            </div>
                            ) : (
                            <></>
                        )}
                        
                    </div>
                </div>
            </Modal.Body>
        </Modal>
    );
};

AnswerViewer.propTypes = {
    data: PropTypes.object.isRequired,
};

const styles = {
    container: {
        border: "1px solid #000",
        borderRadius: "5px",
        padding: "10px",
        maxWidth: "400px",
        margin: "5px auto",
    },
    field: {
        display: "flex",
        justifyContent: "space-between",
        borderBottom: "1px dashed #666",
        padding: "5px 0",
    },
    fieldName: {
        fontWeight: "bold",
    },
    fieldValue: {
        textAlign: "right",
    },
};

export default AnswerViewer;