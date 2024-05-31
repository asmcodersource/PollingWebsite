import React, { useEffect, useState } from 'react';
import { BaseQuestion, BaseResponse } from './QuizQuestion'

export interface SelectQuestion extends BaseQuestion {
    defaultValue: string,
    options: string[],
}

export interface SelectResponse extends BaseResponse {
    text: string,
}

const SelectQuestionComponent = (props: any) => {
    let defaultResponse: SelectResponse = {
        text: props.question.defaultValue ?? props.question.options[0] ?? '',
        questionId: props.question.id,
    }
    const [questionProps, setQuestionProps] = useState<SelectQuestion>(props.question);
    const [response, setResponse] = useState<SelectResponse>(defaultResponse);

    function updateResponse(text: string) {
        let newResponse = { ...response, text: text };
        props.responseDictionary?.set(response.questionId, newResponse);
        setResponse(newResponse);
    }

    useEffect(
        () => {
            updateResponse(props.question.defaultValue ?? questionProps.options[0] ?? '');
        }
        ,[]
    )

    return (
        <div className="select-question-wrapper">
            <label className="description">{questionProps.description}</label>
            <select value={response.text} onChange={(e) => updateResponse(e.target.value)}>
                {questionProps.options?.map((option, index) =>
                    <option key={index}>{option}</option>
                )}
            </select>
        </div>
    )
}

export default SelectQuestionComponent;