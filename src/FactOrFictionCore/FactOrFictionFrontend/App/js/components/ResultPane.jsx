import React from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import Button from './Button';
import Sentence from './Sentence';
import { VIEW_INPUT } from '../constants/viewTypes';
import _ from '../../stylesheets/components/_ResultPane.scss';
import { Progress } from 'react-sweet-progress';
import "react-sweet-progress/lib/style.css";

export default class ResultPane extends React.Component {
    static propTypes = {
        selectedEntryId: PropTypes.string.isRequired,
        selectEntry: PropTypes.func.isRequired,
        textEntryTokens: PropTypes.array.isRequired,
        changeView: PropTypes.func.isRequired,
        similarTokenIds: PropTypes.object.isRequired,
        fetchSimilarTokens: PropTypes.func.isRequired
    }
    
    render() {
        const { 
            selectedEntryId, 
            selectEntry, 
            textEntryTokens, 
            changeView, 
            similarTokenIds,
            fetchSimilarTokens
        } = this.props;

        var objectiveCount = textEntryTokens.filter(e => e.type == "OBJECTIVE").length;
        var percentObjective = parseFloat(objectiveCount * 100.0 / textEntryTokens.length).toFixed(1);

        return (
            <div>
                <div className="left-bar">
                    <div className="result-box" id="result-box">
                    {
                        textEntryTokens.map(entry => (
                            <Sentence
                                {...entry}
                                selectedEntryId={selectedEntryId}
                                selectEntry={selectEntry}
                                key={shortid.generate()}   
                                similarTokenIds={similarTokenIds} 
                                fetchSimilarTokens={fetchSimilarTokens}
                            />
                            )
                        )
                    }
                    </div>
                </div>
                <Progress 
                    percent={percentObjective} 
                    status="success" 
                    theme={{
                        success: {
                          symbol: percentObjective + '%',
                          color: '#7cbb00'
                        }
                    }}
                />
                <Button 
                    handleClick={() => changeView(VIEW_INPUT)} 
                    content="View Input"
                />
            </div>
        );
    }
}
